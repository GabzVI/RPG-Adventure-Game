using UnityEngine;
using System.Collections;
using System;

namespace RPG.Stats
{
	public class Experience : MonoBehaviour
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
	}
}

