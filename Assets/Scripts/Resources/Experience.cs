using UnityEngine;
using System.Collections;

namespace RPG.Resources
{
	public class Experience : MonoBehaviour
	{
		[SerializeField] float experiencePoints = 0f;

		public void GainExperience(float experience)
		{
			experiencePoints += experience;
		}
	
	}
}

