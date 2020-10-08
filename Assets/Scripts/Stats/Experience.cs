using UnityEngine;
using System.Collections;
using System;
using RPG.Saving;

namespace RPG.Stats
{
	public class Experience : MonoBehaviour, ISaveable
	{
		[SerializeField] float experiencePoints = 0f;
		

		public event Action onExperienceGained;
		float MaxExperiencePoint = 0;

		public void GainExperience(float experience)
		{
			experiencePoints += experience;
			onExperienceGained();
		}

		public float GetExperiencePoints()
		{
			return experiencePoints;
		}

		public object CaptureState()
		{
			return experiencePoints;
		}

		public void RestoreState(object state)
		{
			experiencePoints = (float)state;
		}
	}
}

