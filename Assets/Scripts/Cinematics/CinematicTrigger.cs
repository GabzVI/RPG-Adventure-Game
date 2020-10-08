using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Movement;

namespace RPG.Cinematics
{
	//In order for this to work use UnityEngine.Playables with allows us to access our cinematics and use it.
	public class CinematicTrigger : MonoBehaviour
	{
		public bool alreadyTriggered = false;

		private void OnTriggerEnter(Collider other)
		{
			if (!alreadyTriggered && other.gameObject.tag == "Player")
			{
				alreadyTriggered = true;
				print("Play cinematic");
				GetComponent<PlayableDirector>().Play();
			}

		}

	}
}

