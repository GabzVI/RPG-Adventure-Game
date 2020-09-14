using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Stats
{
	public class BaseStats : MonoBehaviour
	{
		[Range(1, 99)]
		[SerializeField] int startingLevel = 1;
		[SerializeField] CharacterClass characterClass;
		[SerializeField] Progression progression = null;
		[SerializeField] GameObject levelUpParticle = null;
		[SerializeField] bool shouldUseModifiers = false;

		public event Action onLevelUp;
		private float XPToLevelUp = 0;
		int currentLevel = 0;
		Experience experience;
		public bool hasLeveledUp = false;

		private void Awake()
		{
		    experience = GetComponent<Experience>();
		}

		private void Start()
		{
			currentLevel = CalculateLevel();
		}

		private void OnEnable()
		{
			if (experience != null)
			{
				print("Updated Level");
				experience.onExperienceGained += UpdateLevel;
			}
		}

		private void OnDisable()
		{
			if (experience != null)
			{
				print("Updated Level");
				experience.onExperienceGained -= UpdateLevel;
			}
		}

		private void UpdateLevel()
		{
			int newLevel = CalculateLevel();
			if(newLevel > currentLevel)
			{
				currentLevel = newLevel;
				LevelUpEffect();
				onLevelUp();
				hasLeveledUp = true;
			}
			else
			{
				hasLeveledUp = false;
			}
		}

		private void LevelUpEffect()
		{
			Instantiate(levelUpParticle, transform);
		}

		public float GetStat(Stat stat)
		{
			return GetBaseStat(stat) + GetAdditiveModifier(stat) * (1 + GetPercentageModifier(stat)/100);
		}

		private float GetBaseStat(Stat stat)
		{
			return progression.GetStat(stat, characterClass, GetLevel());
		}

		private float GetAdditiveModifier(Stat stat)
		{
			if(!shouldUseModifiers) { return 0; }
			float total = 0;
			foreach( IModifierProvider modifierProvider in GetComponents<IModifierProvider>())
			{
				foreach(float modifier in modifierProvider.GetAdditiveModifiers(stat))
				{
					total += modifier;
				}
			}
			return total;
		}

		private float GetPercentageModifier(Stat stat)
		{
			if (!shouldUseModifiers) { return 0; }
			float total = 0;
			foreach (IModifierProvider modifierProvider in GetComponents<IModifierProvider>())
			{
				foreach (float modifier in modifierProvider.GetPercentageModifiers(stat))
				{
					total += modifier;
				}
			}
			return total;
		}

		public int GetLevel()
		{
			if(currentLevel < 1)
			{
				currentLevel = CalculateLevel();
			}
			return currentLevel;
		}

		public float GetMaxExpToLevelUp()
		{
			return XPToLevelUp;
		}

		private int CalculateLevel()
		{
			Experience experience = GetComponent<Experience>();
			if(experience == null) { return startingLevel; }

			float currentXP = experience.GetExperiencePoints();
			int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelup, characterClass);
			for (int level = 1; level <= penultimateLevel; level++)
			{
				XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelup, characterClass, level);
				if(XPToLevelUp > currentXP)
				{
					return level;
				}
			}
			return penultimateLevel + 1;
		}
	}

}
