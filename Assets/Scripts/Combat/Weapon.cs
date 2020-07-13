using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
	[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Spawn New Weapon", order = 0)]
	public class Weapon : ScriptableObject
	{
		[SerializeField] AnimatorOverrideController animOverride = null;
		[SerializeField] GameObject equippedPrefab = null;
		[SerializeField] float weaponDamage = 5f;
		[SerializeField] float weaponRange = 2f;
		[SerializeField] bool isRightHanded = true;
		[SerializeField] Projectiles projectile;

		Transform handtransform;

		const string weaponName = "Weapon";

		public void Spawn(Transform righthand, Transform leftHand, Animator anim)
		{
			DestroyOldWeapon(righthand, leftHand);
			
			if (equippedPrefab != null)
			{
				handtransform = GetTransform(righthand, leftHand);
				GameObject weapon = Instantiate(equippedPrefab, handtransform);
				weapon.name = weaponName;
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

		public void LaunchProjectile(Transform righthand, Transform lefthand, Health target)
		{
			Projectiles projectileInstance = Instantiate(projectile, GetTransform(righthand, lefthand).position, Quaternion.identity);
			projectileInstance.SetTarget(target, weaponDamage);
		}

		public float GetWeaponDamage()
		{
			return weaponDamage;
		}

		public float GetWeaponRange()
		{
			return weaponRange;
		}
	}
}
