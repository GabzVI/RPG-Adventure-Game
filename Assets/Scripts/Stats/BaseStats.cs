using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
	public class BaseStats : MonoBehaviour
	{
		[Range(1, 99)]
		[SerializeField] int startingLevel = 1;
		[SerializeField] CharacterClass characterClass;
		[SerializeField] Progression progression = null;

		Experience experience;

		int currentLevel = 1;

		public void Start()
		{
			currentLevel = CalculateLevel();
			
		}

		public void Update()
		{
			int newLevel = CalculateLevel();
			if(newLevel > currentLevel)
			{
				currentLevel = newLevel;
				print("Leveled UP");
			}
		}

		public float GetStat(Stat stat)
		{
			return progression.GetStat(stat, characterClass, GetLevel());
		}

		public int GetLevel()
		{
			return currentLevel;
		}

		public int CalculateLevel()
		{
			experience = GetComponent<Experience>();

			if (experience == null)
			{
				return startingLevel;
			}

			float currentXP = experience.GetExperiencePoints();
			int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelup, characterClass);
			for (int level = 1; level <= penultimateLevel; level++)
			{
				float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelup, characterClass, level);
				if(XPToLevelUp > currentXP)
				{
					return level;
				}
			}
			return penultimateLevel + 1;
		}
	}

}
