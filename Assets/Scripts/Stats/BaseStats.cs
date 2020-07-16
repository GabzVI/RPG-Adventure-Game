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

		public void Update()
		{
			if(gameObject.tag == "Player")
			{
				print("Level :" + GetLevel());
			}
		}
		public float GetStat(Stat stat)
		{
			return progression.GetStat(stat, characterClass, startingLevel);
		}

		public int GetLevel()
		{
			Experience experience = GetComponent<Experience>();

			if(experience == null) { return startingLevel; }

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
