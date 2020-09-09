using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Resources;
using RPG.Stats;
using RPG.Control;
using System;

namespace RPG.Combat
{
	public class FighterScript : MonoBehaviour, IAction, IModifierProvider
	{

		[SerializeField] float timeBetweenAttacks = 1.0f;
		[SerializeField] Transform rightHandTransform = null;
		[SerializeField] Transform leftHandTransform = null;
		[SerializeField] Weapon defaultWeapon = null;
		[SerializeField] float chaseSpeed = 0f;

		Health target;
		Animator animator;
		Weapon currentWeapon = null;
		GameObject player;
		GameObject[] enemies;
	
		//Will enable characters to attack straight away.
		float timeForLastAttack = Mathf.Infinity;

		private void Awake()
		{
			animator = GetComponent<Animator>();
			player = GameObject.Find("Player");
			enemies = GameObject.FindGameObjectsWithTag("Enemy");
			
			if (currentWeapon == null)
			{
				EquipWeapon(defaultWeapon);
			}
		    
		}

		private void Update()
		{
			timeForLastAttack += Time.deltaTime;

			if (target == null) { return; }
			if (target.IsDead()) { return; }

			if (!GetIsinRange())
			{
				GetComponent<Mover>().MoveTo(target.transform.position, chaseSpeed);
			}
			else 
			{
				GetComponent<Mover>().Cancel();
				AttackBehaviour();
			}
			
		}

		public void EquipWeapon(Weapon weapon)
		{
			currentWeapon = weapon;
			weapon.Spawn(rightHandTransform, leftHandTransform, animator);
			setAttackSpeed();
		}

		private void AttackBehaviour()
		{
			if (timeForLastAttack > timeBetweenAttacks)
			{
				//This will rotate the character towards the enemy that it will hit.
				transform.LookAt(target.transform);

				TriggetAttack();
				timeForLastAttack = 0f;

			}
		}

		private void TriggetAttack()
		{
			//This will reset the stopAttack trigger to false.
			GetComponent<Animator>().ResetTrigger("stopAttack");
			//This will trigger the Hit() event on animation to deal damage.
			GetComponent<Animator>().SetTrigger("Attack");
		}

		public Health GetTarget()
		{
			return target;
		}

		//Animation Event 
		void Hit()
		{
			if(target == null) { return; }

			float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
			
			if (currentWeapon.HasProjectile())
			{
				currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
			}
			else
			{
				target.TakeDamage(gameObject, damage);
			}
		}

		private void setAttackSpeed()
		{
			player.GetComponent<Animator>().SetFloat("animSpeed", currentWeapon.GetWeaponAttackSpeed());
			foreach (GameObject enemy in enemies)
			{
				enemy.GetComponent<Animator>().SetFloat("animSpeed", 1.0f);
			}
		}

		void Shoot()
		{
			Hit();
		}

		private bool GetIsinRange()
		{
			return Vector3.Distance(transform.position, target.transform.position) <= currentWeapon.GetWeaponRange();
		}

		public bool CanAttack(GameObject combatTarget)
		{
			if (combatTarget == null) { return false; }
			Health targetToTest = combatTarget.GetComponent<Health>();
			return targetToTest != null && !targetToTest.IsDead();

		}

		public void Attack(GameObject combatTarget)
		{
			GetComponent<ActionScheduler>().StartAction(this);
			target = combatTarget.GetComponent<Health>();
		}

		private void StopAttack()
		{
			GetComponent<Animator>().ResetTrigger("Attack");
			GetComponent<Animator>().SetTrigger("stopAttack");
		}

		public IEnumerable<float> GetAdditiveModifiers(Stat stat)
		{
			if(stat == Stat.Damage)
			{
				yield return currentWeapon.GetWeaponDamage();
			}
		}

		public IEnumerable<float> GetPercentageModifiers(Stat stat)
		{
			if(stat == Stat.Damage)
			{
				yield return currentWeapon.GetPercentageBonus();
			}
		}

		public void Cancel()
		{
			StopAttack();
			target = null;
			GetComponent<Mover>().Cancel();
		}
	}
}

