using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Saving
{
	[ExecuteAlways]
	public class SaveableEntity : MonoBehaviour
	{
		//This will generate a random Universally unique identifier in which the chances of objects being duplicates is close to 0%.
		//By giving an empty string it will ensure that the object UI doesnt change as its not set yet and stays the same between different scenes.
		[SerializeField] string uniqueIdentifier = "";
		static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();


		public string GetUniqueIdentifier()
		{
			return uniqueIdentifier;
		}

		public object CaptureState()
		{
			Dictionary<string, object> state = new Dictionary<string, object>();
			foreach(ISaveable saveable in GetComponents<ISaveable>())
			{
				//This will convert any component desirable into a string, so we can have a lost of different components desirable saved as a strings.
				state[saveable.GetType().ToString()] = saveable.CaptureState();
			}
			return state;
			
		}

		public void RestoreState(object state)
		{
			Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
			foreach (ISaveable saveable in GetComponents<ISaveable>())
			{
				string typeString = saveable.GetType().ToString();
				if (stateDict.ContainsKey(typeString))
				{
					saveable.RestoreState(stateDict[typeString]);
				}
			}
		}

#if UNITY_EDITOR
		private void Update()
		{
			if (Application.IsPlaying(gameObject)) { return; }
			//By doing this check it will ensure that new objects added to the scene doesnt have the same UUID as the prefab, as they it will no longer set it as its empty.
			if (string.IsNullOrEmpty(gameObject.scene.path)) { return; }

			SerializedObject serializedObj = new SerializedObject(this);
			SerializedProperty property = serializedObj.FindProperty("uniqueIdentifier");

			if(string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
			{
				property.stringValue = System.Guid.NewGuid().ToString();
				serializedObj.ApplyModifiedProperties();
			}

			globalLookup[property.stringValue] = this;
		}


		private bool IsUnique(string candidate)
		{
			if (!globalLookup.ContainsKey(candidate)) { return true; }
			if (globalLookup[candidate] == this) { return true; }

			if (globalLookup[candidate] == null)
			{
				globalLookup.Remove(candidate);
				return true;
			}
			if (globalLookup[candidate].GetUniqueIdentifier() != candidate)
			{
				globalLookup.Remove(candidate);
				return true;
			}

			return false;
		}
#endif
	}
}

