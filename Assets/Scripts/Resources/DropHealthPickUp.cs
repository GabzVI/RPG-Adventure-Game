using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Resources
{
	public class DropHealthPickUp : MonoBehaviour
	{
		[SerializeField] GameObject pickUpObj;
		[SerializeField] float destroyTimer = 10f;
	
		public void SpawnHealthPickUp()
		{
			GameObject prefabToSpawn = Instantiate<GameObject>(pickUpObj, gameObject.transform);
		}

	}
}

