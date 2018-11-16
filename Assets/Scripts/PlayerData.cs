using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    // Класс-контейнер данных, содержит в себе список бонусов,
    // кол-во очков, спиок состояний уровней (уровень закрыт/открыт),
    // список доступного оружия и доступное время для таймера.
	[Serializable]
	public class PlayerData
	{
		public List<Bonus> BonusList { get => bonusList; set => bonusList = value; }
        public float Score { get => score; set => score = value; }
        public List<LevelState> LevelStateList { get => levelStateList; set => levelStateList = value; }
        public List<Weapon> WeaponList { get => weaponList; set => weaponList = value; }
        public float AvailableTime { get => availableTime; set => availableTime = value; }

        [SerializeField]
        private List<Bonus> bonusList;

		[SerializeField]
        private List<Weapon> weaponList;

		[SerializeField]
        private float availableTime, score;

		[SerializeField]
        private List<LevelState> levelStateList;   
    }
}