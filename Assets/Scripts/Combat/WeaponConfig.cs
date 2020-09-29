using System;
using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{
	[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Spawn New Weapon", order = 0)]
	public class WeaponConfig : ScriptableObject
	{
		[SerializeField] AnimatorOverrideController animOverride = null;
		[SerializeField] Weapon equippedPrefab = null;
		[SerializeField] float weaponDamage = 5f;
		[SerializeField] float WeaponAttackSpeed = 1f;
		[SerializeField] float percentageBonus = 0f;
		[SerializeField] float weaponRange = 2f;
		[SerializeField] bool isRightHanded = true;
		[SerializeField] Projectiles projectile;

		Transform handtransform;

		const string weaponName = "Weapon";

		public Weapon Spawn(Transform righthand, Transform leftHand, Animator anim)
		{
			DestroyOldWeapon(righthand, leftHand);

			Weapon weapon = null;
			if (equippedPrefab != null)
			{
				handtransform = GetTransform(righthand, leftHand);
				weapon = Instantiate(equippedPrefab, handtransform);
				weapon.gameObject.name = weaponName;
			}

			var overrideController = anim.runtimeAnimatorController as AnimatorOverrideController;

			if (animOverride != null)
			{
				anim.runtimeAnimatorController = animOverride;
			}
			else if(overrideController != null)
			{
				anim.runtimeAnimatorController = overrideController.runtimeAnimatorController;
			}
			return weapon;
		}

		private void DestroyOldWeapon(Transform righthand, Transform leftHand)
		{
			Transform oldWeapon = righthand.Find(weaponName);
			if(oldWeapon == null)
			{
				oldWeapon =  leftHand.Find(weaponName);
			}
			if(oldWeapon == null) { return; }

			oldWeapon.name = "Destroyed";
			Destroy(oldWeapon.gameObject);
		}

		public Transform GetTransform(Transform righthand, Transform leftHand)
		{
			if (isRightHanded)
			{
				handtransform = righthand;
			}
			else
			{
				handtransform = leftHand;
			}
			return handtransform;
		}

		public bool HasProjectile()
		{
			return projectile != null;
		}

		public void LaunchProjectile(Transform righthand, Transform lefthand, Health target, GameObject instigator, float calculatedDamage)
		{
			Projectiles projectileInstance = Instantiate(projectile, GetTransform(righthand, lefthand).position, Quaternion.identity);
			projectileInstance.SetTarget(target, instigator, calculatedDamage);
		}

		public float GetWeaponDamage()
		{
			return weaponDamage;
		}

		public float GetPercentageBonus()
		{
			return percentageBonus;
		}

		public float GetWeaponRange()
		{
			return weaponRange;
		}

		public float GetWeaponAttackSpeed()
		{
			return WeaponAttackSpeed;
		}
	}
}
