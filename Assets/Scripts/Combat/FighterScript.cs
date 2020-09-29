using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Resources;
using RPG.Stats;
using RPG.Control;
using System;
using GameDevTV.Utils;
using RPG.Saving;

namespace RPG.Combat
{
	public class FighterScript : MonoBehaviour, IAction, IModifierProvider, ISaveable
	{

		[SerializeField] float timeBetweenAttacks = 1.0f;
		[SerializeField] Transform rightHandTransform = null;
		[SerializeField] Transform leftHandTransform = null;
		[SerializeField] WeaponConfig defaultWeapon = null;
		[SerializeField] float chaseSpeed = 0f;

		Health target;
		Animator animator;
		WeaponConfig currentWeaponConfig;
		LazyValue<Weapon> currentWeapon;
		GameObject player;
		GameObject[] enemies;
	
		//Will enable characters to attack straight away.
		float timeForLastAttack = Mathf.Infinity;

		private void Awake()
		{
			animator = GetComponent<Animator>();
			player = GameObject.Find("Player");
			enemies = GameObject.FindGameObjectsWithTag("Enemy");
			currentWeaponConfig = defaultWeapon;
			currentWeapon = new LazyValue<Weapon>(SetUpDefaultWeapon);
		    
		}

		private Weapon SetUpDefaultWeapon()
		{
			return AttachWeapon(defaultWeapon);
		}

		private void Start()
		{
			currentWeapon.ForceInit();
		}
		
		private void Update()
		{
			timeForLastAttack += Time.deltaTime;

			if (target == null) { return; }
			if (target.IsDead()) { return; }

			if (!GetIsinRange(target.transform))
			{
				GetComponent<Mover>().MoveTo(target.transform.position, chaseSpeed);
			}
			else 
			{
				GetComponent<Mover>().Cancel();
				AttackBehaviour();
			}
			
		}

		public void EquipWeapon(WeaponConfig weapon)
		{
			currentWeaponConfig = weapon;
			currentWeapon.value = AttachWeapon(weapon);
		}

		private Weapon AttachWeapon(WeaponConfig weapon)
		{
			setAttackSpeed();
			return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
		}

		private void AttackBehaviour()
		{
			if (timeForLastAttack > timeBetweenAttacks)
			{
				//This will rotate the character towards the enemy that it will hit.
				transform.LookAt(target.transform.position);

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

			if(currentWeapon.value != null)
			{
				currentWeapon.value.OnHit();
			}
			
			if (currentWeaponConfig.HasProjectile())
			{
				currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
			}
			else
			{
				target.TakeDamage(gameObject, damage);
			}
		}

		private void setAttackSpeed()
		{
			player.GetComponent<Animator>().SetFloat("animSpeed", currentWeaponConfig.GetWeaponAttackSpeed());
			foreach (GameObject enemy in enemies)
			{
				enemy.GetComponent<Animator>().SetFloat("animSpeed", 1.0f);
			}
		}

		void Shoot()
		{
			Hit();
		}

		private bool GetIsinRange(Transform targetTrasform)
		{
			return Vector3.Distance(transform.position, targetTrasform.position) <= currentWeaponConfig.GetWeaponRange();
		}

		public bool CanAttack(GameObject combatTarget)
		{
			if (combatTarget == null) { return false; }
			if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) &&
				!GetIsinRange(combatTarget.transform))
			{
				return false;
			}

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
				yield return currentWeaponConfig.GetWeaponDamage();
			}
		}

		public IEnumerable<float> GetPercentageModifiers(Stat stat)
		{
			if(stat == Stat.Damage)
			{
				yield return currentWeaponConfig.GetPercentageBonus();
			}
		}

		public void Cancel()
		{
			StopAttack();
			target = null;
			GetComponent<Mover>().Cancel();
		}

		public object CaptureState()
		{
			return currentWeaponConfig.name;
		}

		public void RestoreState(object state)
		{
			string weaponName = (string)state;
			WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
			EquipWeapon(weapon);

		}
	}
}

