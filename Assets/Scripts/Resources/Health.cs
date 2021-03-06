﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;
using RPG.Core;
using RPG.Movement;
using System;
using UnityEngine.Events;
using GameDevTV.Utils;
using RPG.Saving;
using RPG.Menu;
using RPG.SceneManagement;


namespace RPG.Resources
{
	public class Health : MonoBehaviour, ISaveable
	{
		[SerializeField] float regenHealthPercentage = 80f;
		[SerializeField] float unitHP = 0f;
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
			unitHP = healthPoints.value;
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
				onDie?.Invoke();
				if(gameObject.tag == "Enemy")
				{
					GetComponent<DropHealthPickUp>().SpawnHealthPickUp();
				}
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

		public void SetHealthPoints(float _v)
		{
			healthPoints.value = _v;
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

			if (gameObject.tag == "Player")
			{
				RPG.SceneManagement.SavingWrapper savingWrapper = FindObjectOfType<RPG.SceneManagement.SavingWrapper>();
				savingWrapper.Save();
				GameObject.FindGameObjectWithTag("Respawn").GetComponent<GameManager>().Restart();
				isDead = false;
			}

		}

		private void RegenereteHealth()
		{
			healthPoints.value = Mathf.Max(healthPoints.value, GetMaxHealthPoints() * (regenHealthPercentage / 100));
		}

		public void PickUpRegen(float pickUpRegenPercentage)
		{
			healthPoints.value += ((pickUpRegenPercentage / GetMaxHealthPoints()) * 100);
		}

		public object CaptureState()
		{
			return healthPoints.value;
		}

		public void RestoreState(object state)
		{
			healthPoints.value = (float)state;
			if(healthPoints.value <= 0)
			{
				Die();
			}
		}
	}
}

