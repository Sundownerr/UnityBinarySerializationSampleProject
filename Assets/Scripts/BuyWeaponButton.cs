using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	// Класс для кнопки покупки оружия,
	// в редакторе задаются параметры оружия
	public class BuyWeaponButton : MonoBehaviour 
	{
		public Weapon Weapon;

		private Button button;

		private void Start()
		{
			button = transform.GetComponent<Button>();
			button.onClick.AddListener(Buy);	
			button.GetComponentInChildren<Text>().text = Weapon.Name;	

			var playerWeaponList = Main.Instance.PlayerData.WeaponList;

			// Проверка у игрока наличие оружия, который содержит кнопка.
			// Если такое оружие есть, то кнопка становится некликабельной.
			for (int i = 0; i < playerWeaponList.Count; i++)
				if(playerWeaponList[i].Id == Weapon.Id)
				{
					button.interactable = false;
					break;
				}			
		}

		// Метод, который проверяет, есть ли у игрока нужное кол-во
		// очков и вызывает в Main метод покупки.
		private void Buy()
		{
			if(Main.Instance.PlayerData.Score >= Weapon.Cost)
			{
				Main.Instance.BuyWeapon(Weapon);	
				button.interactable = false;
			}
		}	
	}
}
