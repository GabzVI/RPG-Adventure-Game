using UnityEngine;
using System.Collections.Generic;
using System;

namespace RPG.Stats
{
	[CreateAssetMenu(fileName = "ProgressionStats", menuName = "Stats/New Progression Stats", order = 0)]
	public class Progression : ScriptableObject
	{

		[SerializeField] ProgressionCharacterClass[] characterClasses = null;

		Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

		//In order to use a class within a class you must tell unity to serialize the class by using system.serializable
		[System.Serializable]
		class ProgressionCharacterClass
		{
			public CharacterClass characterClass;
			public ProgressionStat[] stats;
		}

		[System.Serializable]
		class ProgressionStat
		{
			public Stat stat;
			public float[] levels;
		}

	    public float GetStat(Stat stat, CharacterClass characterClass, int level)
		{
			//Dictionary is more efficient than using foreach loops as its only built once and can be accessed multiple times instead of
			//having to reloop multiples times reducing performance.
			BuildLookUP();

			float[] levels = lookupTable[characterClass][stat];

			if(levels.Length < level) { return 0; }

			return levels[level - 1];

		}

		public int GetLevels(Stat stat, CharacterClass characterClass)
		{
			BuildLookUP();

			float[] levels = lookupTable[characterClass][stat];
			return levels.Length;
		}

		private void BuildLookUP()
		{
			if(lookupTable != null) { return; }
			lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

			foreach (ProgressionCharacterClass progressionClass in characterClasses)
			{
				var statLookupTable = new Dictionary<Stat, float[]>();

				foreach (ProgressionStat progressionStat in progressionClass.stats)
				{
					statLookupTable[progressionStat.stat] = progressionStat.levels;
				}
				lookupTable[progressionClass.characterClass] = statLookupTable;
			}
		}
	}

}
