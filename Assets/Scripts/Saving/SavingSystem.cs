﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
	public class SavingSystem : MonoBehaviour
	{
		public IEnumerator LoadLastScene(string saveFile)
		{
			Dictionary<string, object> state = LoadFile(saveFile);
			if (state.ContainsKey("lastSceneBuildIndex"))
			{
				int buildIndex = (int)state["lastSceneBuildIndex"];
				if (buildIndex != SceneManager.GetActiveScene().buildIndex)
				{
					yield return SceneManager.LoadSceneAsync(buildIndex);
				}
			}
			RestoreState(state);
		}

		public void Save(string saveFile)
		{
			Dictionary<string, object> state = LoadFile(saveFile);
			CaptureState(state);
			SaveFile(saveFile, state);
		}

		public void Load(string saveFile)
		{
			RestoreState(LoadFile(saveFile));
		}

		private void SaveFile(string saveFile, object state)
		{
			string path = GetPathFromSaveFile(saveFile);
			print("Saving From" + path);
			//By using the "using" it will ensure that the filestream gets closed and doesnt continue running.
			using (FileStream stream = File.Open(path, FileMode.Create))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, state);
			}
		}

		private Dictionary<string, object> LoadFile(string saveFile)
		{
			string path = GetPathFromSaveFile(saveFile);
			print("Loading From: " + path);
			if (!File.Exists(path))
			{
				return new Dictionary<string, object>();
			}
			using (FileStream stream = File.Open(path, FileMode.Open))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				return (Dictionary<string, object>)formatter.Deserialize(stream);
			}
		}

		private void CaptureState(Dictionary<string, object> state)
		{
			foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
			{
				state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
			}

			state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
		}

		private void RestoreState(Dictionary<string, object> state)
		{
			foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
			{
				string id = saveable.GetUniqueIdentifier();
				if (state.ContainsKey(id))
				{
					saveable.RestoreState(state[id]);
				}
			}
		}

		private string GetPathFromSaveFile(string saveFile)
		{
			return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
		}
	}
}

