using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game
{
	// Базовый класс оружия - содержит в себе название оружия,
	// случайное значение (будь то +10 к урону, скорости атаки и т.п.),
	// цену в очках и идентификатор для сравнения.
	[Serializable]
	public class Weapon 
	{
		public string Name { get => name; set => name = value; }
        public float SomeNumber { get => someNumber; set => someNumber = value; }
        public float Cost { get => cost; set => cost = value; }
        public float Id { get => id; set => id = value; }

        [SerializeField]
        private string name;

		[SerializeField]
        private float someNumber, cost, id;
		
		// Метод для применения оружия к данным игрока
		public virtual void Apply(ref PlayerData playerData)
		{
			// ...
		}
	}
}