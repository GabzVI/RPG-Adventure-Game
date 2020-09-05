﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;
using RPG.Core;
using RPG.Movement;
using System;
using UnityEngine.Events;

namespace RPG.Resources
{
	public class Health : MonoBehaviour
	{
		[SerializeField] float regenHealthPercentage = 80f;
		[SerializeField] TakeDamageEvent takeDamage;

		//Inheriting from Unityevent and has float
		[System.Serializable]
		public class TakeDamageEvent : UnityEvent<float>
		{

		}

		public float healthPoints;

		BaseStats basestats;
		bool isDead = false;

		public bool IsDead()
		{
			return isDead;
		}

		private void Start()
		{
			basestats = GetComponent<BaseStats>();
			basestats.onLevelUp += RegenereteHealth;
			healthPoints = basestats.GetStat(Stat.Health);
		}

		public void TakeDamage(GameObject instigator, float damage)
		{
			print(gameObject + "took damage: " + damage);

			//This will ensure that health doesnt go below 0
			healthPoints = Mathf.Max(healthPoints - damage,0);
			

			print("health = " + healthPoints + gameObject.name);
			if(healthPoints == 0)
			{
				Die();
				AwardExp(instigator);
			}
			{
				takeDamage.Invoke(damage);
			}
		}

		public float GetHealthPoints()
		{
			return healthPoints;
		}

		public float GetMaxHealthPoints()
		{
			return basestats.GetStat(Stat.Health);
		}

		private void AwardExp(GameObject instigator)
		{
			Experience experience = instigator.GetComponent<Experience>();
			if(experience == null) { return; }
			experience.GainExperience(basestats.GetStat(Stat.ExperienceReward));
		}

		private void Die()
		{
			if (isDead) { return; }

			isDead = true;

			Destroy(GetComponent<Rigidbody>());
			GetComponent<Animator>().SetTrigger("die");
			GetComponent<ActionScheduler>().CancelCurrentAction();
		}

		private void RegenereteHealth()
		{
			print("Stat.Health = " + basestats.GetStat(Stat.Health));
			healthPoints = Mathf.Max(healthPoints, basestats.GetStat(Stat.Health) * (regenHealthPercentage / 100));
		}
	}
}

