using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;
using RPG.SceneManagement;
using RPG.Resources;
using RPG.Core;

namespace RPG.Menu
{
	public class GameManager : MonoBehaviour
	{
		bool gameHasEnded = false;
		[SerializeField] float restartDelay = 0;
		[SerializeField] Canvas canvas;

		Health playerHealthScript;

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.tag == "Player")
			{
				EndGame();
			}
		}

		public void EndGame()
		{
			gameHasEnded = true;
			if (gameHasEnded == true)
			{
				canvas.gameObject.SetActive(true);
				Debug.Log("Game Over");
			}
			
		}

		public void Restart()
		{
			playerHealthScript = GameObject.FindWithTag("Player").GetComponent<Health>();
			SavingWrapper savingWrapper = FindObjectOfType<RPG.SceneManagement.SavingWrapper>();

			savingWrapper.Load();
			GameObject.FindWithTag("Player").GetComponent<Animator>().ResetTrigger("die");
			GameObject.FindWithTag("Player").GetComponent<NavMeshAgent>().enabled = true;
			GameObject.FindObjectOfType<CameraMovement>().RecenterPlayer();
			playerHealthScript.SetHealthPoints(playerHealthScript.GetMaxHealthPoints());
		}
	}
}

