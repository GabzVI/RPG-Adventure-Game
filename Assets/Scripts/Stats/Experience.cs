using UnityEngine;
using System.Collections;

namespace RPG.Stats
{
	public class Experience : MonoBehaviour
	{
		[SerializeField] float experiencePoints = 0f;

		public void GainExperience(float experience)
		{
			experiencePoints += experience;
		}

		public float GetExperiencePoints()
		{
			return experiencePoints;
		}
	
	}
}

