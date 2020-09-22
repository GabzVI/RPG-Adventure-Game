using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.UI;

namespace RPG.Resources
{
	public class HealthPickUp : MonoBehaviour
	{
		[SerializeField] float pickUpHealthPercentage = 30f;
		[SerializeField] GameObject destroyPrefab;
		
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.tag == "Player")
			{
				other.GetComponent<Health>().PickUpRegen(pickUpHealthPercentage);
				Destroy(destroyPrefab);
			}
		}
	}
}

