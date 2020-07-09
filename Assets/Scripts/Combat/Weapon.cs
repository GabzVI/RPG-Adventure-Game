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

		Transform handtransform;

		public void Spawn(Transform righthand, Transform leftHand, Animator anim)
		{
			if (equippedPrefab != null)
			{

				if (isRightHanded)
				{
					handtransform = righthand;
				}
				else
				{
					handtransform = leftHand;
				}
				Instantiate(equippedPrefab, handtransform);
			}
			if(animOverride != null)
			{
				anim.runtimeAnimatorController = animOverride;
			}
			
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
