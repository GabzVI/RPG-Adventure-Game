using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Saving;

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
			if(sceneToLoad < 0)
			{
				Debug.LogError("Scene to Load has not been set");
				yield break;
			}


			

			//This will be used to not delete the portal after the user has gone to the other scene as coroutine deletes it automatically.
			DontDestroyOnLoad(gameObject);

			Fader fader = FindObjectOfType<Fader>();

			yield return fader.FadeOut(FadeOutTimer);

			//Save scene
			SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
			wrapper.SaveScene();

			yield return SceneManager.LoadSceneAsync(sceneToLoad);

			wrapper.LoadScene();

			Portal otherPortal = GetOtherPortal();
			UpdatePlayer(otherPortal);

			wrapper.SaveScene();

			yield return new WaitForSeconds(FadeWaitTimer);
			yield return fader.FadeIn(FadeInTimer);
			
			Destroy(gameObject);
		}

		private void UpdatePlayer(Portal otherPortal)
		{
			GameObject player = GameObject.FindWithTag("Player");

			player.GetComponent<NavMeshAgent>().enabled = false;
			//Warp sets the navmesh of the player to be equal to the position of the spawn point.
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

