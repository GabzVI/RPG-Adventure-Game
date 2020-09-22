using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;
using RPG.Core;
using RPG.Movement;
using System;
using UnityEngine.Events;
using GameDevTV.Utils;

namespace RPG.Resources
{
	public class Health : MonoBehaviour
	{
		[SerializeField] float regenHealthPercentage = 80f;
		[SerializeField] TakeDamageEvent takeDamage;
		[SerializeField] UnityEvent onDie;

		//Inheriting from Unityevent and has float
		[System.Serializable]
		public class TakeDamageEvent : UnityEvent<float>
		{

		}

		LazyValue<float> healthPoints;
		
		Health target;

		bool isDead = false;

		public bool IsDead()
		{
			return isDead;
		}

		private void Awake()
		{
			//This will initialize it, before we are able to use to make it safer to use.
			healthPoints = new LazyValue<float>(GetInitialHealth);
			GetComponent<BaseStats>().onLevelUp += RegenereteHealth;
		}

		private void Start()
		{
			//This will force to initialize the variable.
			healthPoints.ForceInit();
		}

		private void Update()
		{
			if (healthPoints.value > GetMaxHealthPoints())
			{
				healthPoints.value = GetMaxHealthPoints();
			}
		}

		private float GetInitialHealth()
		{
			return GetComponent<BaseStats>().GetStat(Stat.Health);
		}


		public void TakeDamage(GameObject instigator, float damage)
		{

			//This will ensure that health doesnt go below 0
			healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
			if (healthPoints.value == 0)
			{
				onDie.Invoke();
				GetComponent<DropHealthPickUp>().SpawnHealthPickUp();
				Die();
				AwardExp(instigator);
			}
			else
			{
				takeDamage.Invoke(damage);
			}
		}

		public float GetHealthPoints()
		{
			return healthPoints.value;
		}

		public float GetMaxHealthPoints()
		{
			return GetComponent<BaseStats>().GetStat(Stat.Health);
		}

		public float GetHealthPercentage()
		{
			return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
		}

		private void AwardExp(GameObject instigator)
		{
			Experience experience = instigator.GetComponent<Experience>();
			if (experience == null) { return; }
			experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
		}

		private void Die()
		{
			if (isDead) { return; }

			isDead = true;

			GetComponent<Animator>().SetTrigger("die");
			GetComponent<ActionScheduler>().CancelCurrentAction();
			
		}

		private void RegenereteHealth()
		{
			healthPoints.value = Mathf.Max(healthPoints.value, GetMaxHealthPoints() * (regenHealthPercentage / 100));
		}

		public void PickUpRegen(float pickUpRegenPercentage)
		{
			healthPoints.value += ((pickUpRegenPercentage / GetMaxHealthPoints()) * 100);
		}
	}
}

