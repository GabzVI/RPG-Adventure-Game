using UnityEngine;
using System.Collections;
using RPG.Resources;
using RPG.Control;

namespace RPG.Combat
{
	[RequireComponent(typeof(Health))]
	public class CombatTarget : MonoBehaviour, IRaycastable
	{
		public bool HandleRaycast(PlayerController callingController)
		{
			//Continue is to continue loop and check for other objects until target isnt null is found.
			if (!callingController.GetComponent<FighterScript>().CanAttack(gameObject))
			{
				return false;
			}

			if (Input.GetMouseButton(1))
			{
				callingController.GetComponent<FighterScript>().Attack(gameObject);
			}
			return true;
		}

		public CursorType GetCursorType()
		{
			return CursorType.Combat;
		}
	}
}

