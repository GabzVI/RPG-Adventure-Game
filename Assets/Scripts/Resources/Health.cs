using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;
using RPG.Core;

namespace RPG.Resources
{
	public class Health : MonoBehaviour
	{
		public float healthPoints;

		BaseStats basestats;
		bool isDead = false;

		public bool IsDead()
		{
			return isDead;
		}

		private void Start()
		{
			healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);	
		}
		 
		public void TakeDamage(GameObject instigator, float damage)
		{
			//This will ensure that health doesnt go below 0
			healthPoints = Mathf.Max(healthPoints - damage,0);
			print("health = " + healthPoints + gameObject.name);
			if(healthPoints == 0)
			{
				Die();
				AwardExp(instigator);
			}
		}

		public float GetHealthPercentage()
		{
			return 100 * (healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health));
		}

		private void AwardExp(GameObject instigator)
		{
			Experience experience = instigator.GetComponent<Experience>();
			if(experience == null) { return; }
			experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
		}

		private void Die()
		{
			if (isDead) return;

			isDead = true;
			GetComponent<Animator>().SetTrigger("die");
			GetComponent<ActionScheduler>().CancelCurrentAction();
		}

	}
}

