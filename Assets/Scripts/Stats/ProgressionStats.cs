using UnityEngine;

namespace RPG.Stats
{
	[CreateAssetMenu(fileName = "ProgressionStats", menuName = "Stats/New Progression Stats", order = 0)]
	public class ProgressionStats : ScriptableObject
	{

		[SerializeField] ProgressionCharacterClass[] characterClasses = null;

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
			public float[] Level;
		}

	    public float GetHealth(CharacterClass characterClass, int level)
		{
			//The foreach loop is going through the different classes in characterClasses
			//and looking for a character class in progressionStats that
			// matches with the one in base stats, then sets the health of the character to the array/level of character.
			foreach(ProgressionCharacterClass progressionClass in characterClasses)
			{
				if(progressionClass.characterClass == characterClass)
				{
					//return progressionClass.health[level - 1];
				}
			}
			return 0;
		}

		
	}

}
