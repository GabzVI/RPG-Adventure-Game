using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
	public class PersistentObj : MonoBehaviour
	{
		[SerializeField] GameObject poPrefab;

		static bool hasSpawned = false;

		private void Awake()
		{
			if (hasSpawned) { return; }

			SpawnPersistentObjs();

			hasSpawned = true;
		}

		private void SpawnPersistentObjs()
		{
			GameObject persistentObj = Instantiate(poPrefab);
			DontDestroyOnLoad(persistentObj);
		}
	}

}
