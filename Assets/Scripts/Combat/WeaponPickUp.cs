using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;

namespace RPG.Combat
{
	public class WeaponPickUp : MonoBehaviour, IRaycastable
	{
		[SerializeField] Weapon weapon = null;
		[SerializeField] float respawnTime = 5f;

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.tag == "Player")
			{
				PickUp(other.GetComponent<FighterScript>());
			}
		}

		private void PickUp(FighterScript fighter)
		{
			fighter.EquipWeapon(weapon);
			StartCoroutine(HidePickUpsOverTime(respawnTime));
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

		public bool HandleRaycast(PlayerController callingController)
		{
			if (Input.GetMouseButtonDown(1))
			{
				//Checks if the character has a fighterscript.
				PickUp(callingController.GetComponent<FighterScript>());
			}
			return true;
		}

		public CursorType GetCursorType()
		{
			return CursorType.Pickup;
		}
	}
}

