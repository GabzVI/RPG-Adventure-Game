using RPG.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
	public class SavingWrapper : MonoBehaviour
	{

		const string defaultSaveFile = "save";

		private IEnumerator Start()
		{
			Fader fader = FindObjectOfType<Fader>();

			yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
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

