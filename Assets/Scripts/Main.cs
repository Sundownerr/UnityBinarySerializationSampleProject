using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Game
{
	[Serializable]
	public enum LevelState
	{
		Locked,
		Unlocked
	}


	public class Main : MonoBehaviour 
	{
		public static Main Instance;

		public PlayerData PlayerData { get => playerData; set => playerData = value; }
		public Button AddScoreButton, RunTimerButton;
		public Text ScoreText, TimerText;
		public Text[] LevelStateList;
		public List<int> ScoreToUnlockLevelList;

		private bool isTimerStarted;
		private PlayerData playerData;
		private string saveFilePath ;
		private double minutes, seconds;

        private void Awake()
		{
			// Путь сохранения и загрузки данных, на телефоне находится в Android/data
			saveFilePath = Application.persistentDataPath + "test.dat";

			if (Instance == null)
                Instance = this;

			AddScoreButton.onClick.AddListener(AddScore);	
			RunTimerButton.onClick.AddListener(RunTimer);	

			// Загрузка (или создание, если данных нет) данных об игроке
			playerData = LoadPlayerData();							
			
			UpdateUI();	
		}


		private void Update()
		{
			// Реализация таймера, в вашем проекте не нужна.
			if(isTimerStarted)
			{					
				seconds -= Time.deltaTime;

				if(seconds <= 0)				
					if(minutes < 0)
						isTimerStarted = false;					
					else
					{
						seconds = 60;
						minutes--;
					}								
				TimerText.text = $"{minutes}:{Math.Truncate(seconds)}";							
			}
		}

		// Метод для запуска таймера, в вашем проекте не нужен.
		private void RunTimer() => isTimerStarted = true;
	
		// Метод добавления очков, вызывается путем нажатия на кнопку AddScoreButton.
		private void AddScore()
		{
			playerData.Score += 10;			
			CheckLevelUnlock();
			SavePlayerData();
		}

		// Метод покупки бонусов - добавляет бонус в список бонусов игрока,
		// отнимает кол-во очков, которое он стоит, и применяет его к данным игрока.
		public void BuyBonus(Bonus bonus)
		{	
			playerData.BonusList.Add(bonus);			
			playerData.Score -= bonus.Cost;
			bonus.Apply(ref playerData);
			SavePlayerData();		
		}

		// Метод покупки оружия - добавляет оружие в список оружия игрока,
		// отнимает кол-во очков, которое оно стоит, и применяет его к данным игрока.
		public void BuyWeapon(Weapon weapon)
		{			
			playerData.WeaponList.Add(weapon);
			playerData.Score -= weapon.Cost;
			weapon.Apply(ref playerData);
			SavePlayerData();			
		}

		// Предикатный метод, который проверяет достигло ли кол-во
		// очков нужного значения для открытия уровня.
		// Значения задаются в редакторе, в объекте Main, в списке Score To Unlock Level List,
		// где кол-во элементов должно быть равно кол-ву уровней, 
		// и каждый элемент содержит в себе значение нужного кол-ва очков для открытия уровня,
		// номер которого соответствует индексу элемента в списке.
		// Т.е в элементе 0 нужно задвать значение для 1 уровня, в 1 для 2-го и т.д.
		private void CheckLevelUnlock()
		{
			for (int i = 0; i < playerData.LevelStateList.Count; i++)	
				if(playerData.LevelStateList[i] == LevelState.Locked)	
					if(playerData.Score >= ScoreToUnlockLevelList[i])				
						playerData.LevelStateList[i] = LevelState.Unlocked;			

			for (int i = 0; i < LevelStateList.Length; i++)				
				LevelStateList[i].text = $"Level {i}: {playerData.LevelStateList[i].ToString()}";
		}

		// Метод обновления интерфейса, в вашем проекте не нужен.
		private void UpdateUI()
		{
			minutes = Math.Truncate((double)playerData.AvailableTime);
			seconds = (int)(((double)playerData.AvailableTime % 1) * 100);	
			
			for (int i = 0; i < LevelStateList.Length; i++)				
				LevelStateList[i].text = $"Level {i}: {playerData.LevelStateList[i].ToString()}";

			ScoreText.text = playerData.Score.ToString();
			TimerText.text = $"{minutes}:{Math.Truncate(seconds)}";
		}

		// Метод для загрузки данных. Проверяет, существует ли файл с данными, если да - 
		// открывает его и десереализует значения, если нет - создает новый
		// файл, создает значения и сереализует их и записывает в файл.
		private PlayerData LoadPlayerData()
		{
			var tempData = new PlayerData();

			if(File.Exists(saveFilePath))			
				using (var saveFile = File.Open(saveFilePath, FileMode.Open))
				{						
					var data = new BinaryFormatter().Deserialize(saveFile) as PlayerData;				
					tempData.LevelStateList = data.LevelStateList;					
					tempData.BonusList 		= data.BonusList;	
					tempData.WeaponList 	= data.WeaponList;
					tempData.Score 			= data.Score;	
					tempData.AvailableTime 	= data.AvailableTime;

					saveFile.Close();
				}				
			else			
				using (var saveFile = File.Create(saveFilePath))
				{	
					tempData.LevelStateList = new List<LevelState>();					
					tempData.BonusList 		= new List<Bonus>();
					tempData.WeaponList 	= new List<Weapon>();
					tempData.Score 			= 0;
					tempData.AvailableTime 	= (float)1.30;
				
					for (int i = 0; i < 5; i++)
						tempData.LevelStateList.Add(LevelState.Locked);				

					new BinaryFormatter().Serialize(saveFile, tempData);
					saveFile.Close();				
				}	
			return tempData;
		}

		// Метод для сохранения данных.
		private void SavePlayerData()
		{
			UpdateUI();
			using (var saveFile = File.Open(saveFilePath, FileMode.Open))
			{						
				new BinaryFormatter().Serialize(saveFile, playerData);
				saveFile.Close();
			}
		}
	}
}