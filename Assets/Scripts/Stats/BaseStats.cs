using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameDevTV.Utils;
using UnityEngine.Events;

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
		[SerializeField] UnityEvent lvlUp;

		public event Action onLevelUp;
		private float XPToLevelUp = 0;
		LazyValue<int> currentLevel;
		Experience experience;

		private void Awake()
		{
		    experience = GetComponent<Experience>();
			currentLevel = new LazyValue<int>(CalculateLevel);
		}

		private void Start()
		{
			currentLevel.ForceInit();
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
			if(newLevel > currentLevel.value)
			{
				currentLevel.value = newLevel;
				LevelUpEffect();
				lvlUp.Invoke();
				onLevelUp();
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
			return currentLevel.value;
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
