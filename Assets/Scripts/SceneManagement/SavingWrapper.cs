using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement
{
	public class SavingWrapper : MonoBehaviour
	{

		const string defaultSaveFile = "save";
		[SerializeField] float fadeinTime = 0.2f;

		private IEnumerator Start()
		{
			Fader fader = FindObjectOfType<Fader>();
			fader.FadeOutQuick();
			yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
			yield return fader.FadeIn(fadeinTime);
		}

		// Update is called once per frame
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.S))
			{
				Save();
			}
			if (Input.GetKeyDown(KeyCode.L))
			{
				Load();
			}
		}

		public void Save()
		{
			GetComponent<SavingSystem>().Save(defaultSaveFile);
		}
		public void Load()
		{
			GetComponent<SavingSystem>().Load(defaultSaveFile);
		}
	}

}

