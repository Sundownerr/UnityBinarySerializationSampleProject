﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	// Класс для кнопки покупки бонуса,
	// в редакторе задаются параметры бонуса
	public class BuyBonusButton : MonoBehaviour {

		public Bonus Bonus;

		private Button button;

		private void Start()
		{
			button = transform.GetComponent<Button>();
			button.onClick.AddListener(Buy);			
			button.GetComponentInChildren<Text>().text = Bonus.Name;	
		
			var playerBonusList = Main.Instance.PlayerData.BonusList;

			// Проверка у игрока наличие бонуса, который содержит кнопка.
			// Если такой бонус есть, то кнопка становится некликабельной.
			for (int i = 0; i < playerBonusList.Count; i++)
				if(playerBonusList[i].Id == Bonus.Id)
				{
					button.interactable = false;
					break;
				}			
		}

		// Метод, который проверяет, есть ли у игрока нужное кол-во
		// очков и вызывает в Main метод покупки.
		private void Buy()
		{
			if(Main.Instance.PlayerData.Score >= Bonus.Cost)
			{
				Main.Instance.BuyBonus(Bonus);		
				button.interactable = false;
			}
		}
	}
}
