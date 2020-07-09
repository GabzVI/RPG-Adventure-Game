using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Core
{
	public class Health : MonoBehaviour, ISaveable
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

		public object CaptureState()
		{
			//Doesnt need to be serializable as by default floats already are.
			return healthPoints;
		}

		public void RestoreState(object state)
		{
			//changes to state to float.
			healthPoints = (float)state;

			//We need to reuse the if statement to kill off the character if it is already dead after we load
			if (healthPoints == 0)
			{
				Die();
			}

		}


	}
}

