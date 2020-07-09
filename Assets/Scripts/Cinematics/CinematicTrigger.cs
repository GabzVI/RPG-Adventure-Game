using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Movement;
using RPG.Saving;

namespace RPG.Cinematics
{
	//In order for this to work use UnityEngine.Playables with allows us to access our cinematics and use it.
	public class CinematicTrigger : MonoBehaviour, ISaveable
	{
		bool alreadyTriggered = false;

		private void OnTriggerEnter(Collider other)
		{
			if (!alreadyTriggered && other.gameObject.tag == "Player")
			{
				alreadyTriggered = true;
				print("Play cinematic");
				GetComponent<PlayableDirector>().Play();
			}

		}

		public object CaptureState()
		{
			return alreadyTriggered;
			
		}

		public void RestoreState(object state)
		{
			alreadyTriggered = (bool)state;
		}
	}
}

