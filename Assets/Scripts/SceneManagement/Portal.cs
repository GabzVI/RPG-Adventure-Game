using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Control;
using RPG.Core;

namespace RPG.SceneManagement
{
	public class Portal : MonoBehaviour
	{

		enum PortalIdentifier
		{
			A, B, C, D, E, F
		}

		[SerializeField] int sceneToLoad = -1;
		[SerializeField] Transform SpawnPoint;
		[SerializeField] PortalIdentifier portalDestination;
		[SerializeField] float FadeInTimer = 1f;
		[SerializeField] float FadeOutTimer = 2f;
		[SerializeField] float FadeWaitTimer = 0.5f;

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.tag == "Player")
			{
				StartCoroutine(TransitionScene());
			}
		
		}

		private IEnumerator TransitionScene()
		{
			if (sceneToLoad < 0)
			{
				Debug.LogError("Scene to Load has not been set");
				yield break;
			}
		

			//This will be used to not delete the portal after the user has gone to the other scene as coroutine deletes it automatically.
			DontDestroyOnLoad(gameObject);
			Fader fader = FindObjectOfType<Fader>();
			SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
			PlayerController playerControl = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
			playerControl.enabled = false;

			yield return fader.FadeOut(FadeOutTimer);

			savingWrapper.Save();

			yield return SceneManager.LoadSceneAsync(sceneToLoad);
			PlayerController newplayerControl = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
			newplayerControl.enabled = false;

			savingWrapper.Load();

			Portal otherPortal = GetOtherPortal();			
			UpdatePlayer(otherPortal);

			savingWrapper.Save();

			yield return new WaitForSeconds(FadeWaitTimer);
			fader.FadeIn(FadeInTimer);

			newplayerControl.enabled = true;
			Destroy(gameObject);
		}

		private void UpdatePlayer(Portal otherPortal)
		{
			GameObject player = GameObject.FindWithTag("Player");
			//Warp sets the navmesh of the player to be equal to the position of the spawn point.
			player.GetComponent<NavMeshAgent>().enabled = false;
			player.GetComponent<NavMeshAgent>().Warp(otherPortal.SpawnPoint.position);
			player.GetComponent<NavMeshAgent>().enabled = true;
		}

		private Portal GetOtherPortal()
		{
			foreach (Portal portal in FindObjectsOfType<Portal>())
			{
				//If portal isnt the one we want in the list skips the portal
				if (portal == this) { continue; }
				//If portal destination isnt the destination we desire will also skip the portal until right destination portal found.
				if (portal.portalDestination != portalDestination) { continue; }
				//This will return current portal form the list.
				return portal;
			}
			//Returns null if no portal is found.
			return null;		
		}
	}
}

