using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Saving
{
	[CreateAssetMenu]
	public class Save : ScriptableObject
	{
		public List<int> items = new();
		public List<int> unlockedPets = new();
		public List<int> equipedItems = new();
		public List<int> equipedGems = new();
		public int equipedPet;
		[Range(1, 5)] public int GemSlotsCount = 1;

		//public eTutorialStep Tutorial;
		public int LastLevel;
		public int Money;
		public Action<int, int> OnMoneyChanged;

		public void AddMoney(int amount)
		{
			int before = Money;
			Money += amount;
			OnMoneyChanged?.Invoke(before, Money);
		}
	}
}