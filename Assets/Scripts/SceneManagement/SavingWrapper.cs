using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement
{
	public class SavingWrapper : MonoBehaviour
	{
		const string defaultSavefile = "Save";
		[SerializeField] float fadeInTime = 1.0f;

		IEnumerator Start()
		{
			Fader fader = FindObjectOfType<Fader>();
			fader.FadeOutQuick();
			yield return GetComponent<SavingSystem>().LoadLastScene(defaultSavefile);
			yield return fader.FadeIn(fadeInTime);
		
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.L))
			{
				LoadScene();
			}
			if (Input.GetKeyDown(KeyCode.S))
			{
				SaveScene();
			}
		}

		public void LoadScene()
		{

			//call to saving system to load the scene
			GetComponent<SavingSystem>().Load(defaultSavefile);
		}

		public void SaveScene()
		{
			GetComponent<SavingSystem>().Save(defaultSavefile);
		}
	}
}

