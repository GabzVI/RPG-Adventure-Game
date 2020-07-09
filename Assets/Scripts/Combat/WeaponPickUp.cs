using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
	public class WeaponPickUp : MonoBehaviour
	{
		[SerializeField] Weapon weapon = null;

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.tag == "Player")
			{
				other.GetComponent<FighterScript>().EquipWeapon(weapon);
				Destroy(gameObject);
			}
		}
	}
}

