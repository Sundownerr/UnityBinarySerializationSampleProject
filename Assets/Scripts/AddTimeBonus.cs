using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	// Дочерний класс от бонуса - бонус, который добавляет время.
	[Serializable]
	public class TimeBonus : Bonus
	{
		public override void Apply(ref PlayerData playerData)
		{
			playerData.AvailableTime += SomeNumber;
		}
	}
}
