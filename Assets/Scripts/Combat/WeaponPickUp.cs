using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
	public class WeaponPickUp : MonoBehaviour
	{
		[SerializeField] Weapon weapon = null;
		[SerializeField] float respawnTime = 5f;

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.tag == "Player")
			{
				other.GetComponent<FighterScript>().EquipWeapon(weapon);
				StartCoroutine(HidePickUpsOverTime(respawnTime));
			}
		}

		private IEnumerator HidePickUpsOverTime(float seconds)
		{
			ShowPickup(false);
			yield return new WaitForSeconds(seconds);
			ShowPickup(true);
		}

		private void ShowPickup(bool ShowPickup)
		{
			GetComponent<Collider>().enabled = ShowPickup;
			foreach(Transform child in transform)
			{
				child.gameObject.SetActive(ShowPickup);
			}
		}
	}
}

