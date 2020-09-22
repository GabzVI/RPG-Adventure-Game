using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.DamageText
{
	public class Destroyer : MonoBehaviour
	{
		[SerializeField] GameObject targetToDestroy = null;
		[SerializeField] float destroyTimer = 0.1f;

		public void DestroyTarget()
		{
			Destroy(targetToDestroy);
		}
	}
}

