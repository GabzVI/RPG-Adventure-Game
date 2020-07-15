using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Core
{
	public class Health : MonoBehaviour
	{
		[SerializeField] float healthPoints = 100f;

		bool isDead = false;

		public bool IsDead()
		{
			return isDead;
		}
		public void TakeDamage(float damage)
		{
			//This will ensure that health doesnt go below 0
			healthPoints = Mathf.Max(healthPoints - damage,0);
			print("health = " + healthPoints + gameObject.name);
			if(healthPoints == 0)
			{
				Die();
			}
		}

		private void Die()
		{
			if (isDead) return;

			isDead = true;
			GetComponent<Animator>().SetTrigger("die");
			GetComponent<ActionScheduler>().CancelCurrentAction();
		}

	}
}

