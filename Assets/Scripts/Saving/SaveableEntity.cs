using RPG.Core;
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
		public string GetUniqueIdentifier()
		{
			return uniqueIdentifier;
		}

		public object CaptureState()
		{
			return new SerializableVector3(transform.position);
		}

		public void RestoreState(object state)
		{
			SerializableVector3 position = (SerializableVector3)state;
			GetComponent<NavMeshAgent>().enabled = false;
			transform.position = position.ToVector();
			GetComponent<NavMeshAgent>().enabled = true;
			GetComponent<ActionScheduler>().CancelCurrentAction();
		}

#if UNITY_EDITOR
		private void Update()
		{
			if (Application.IsPlaying(gameObject)) { return; }
			//By doing this check it will ensure that new objects added to the scene doesnt have the same UUID as the prefab, as they it will no longer set it as its empty.
			if (string.IsNullOrEmpty(gameObject.scene.path)) { return; }

			SerializedObject serializedObj = new SerializedObject(this);
			SerializedProperty property = serializedObj.FindProperty("uniqueIdentifier");
			if(string.IsNullOrEmpty(property.stringValue))
			{
				property.stringValue = System.Guid.NewGuid().ToString();
				serializedObj.ApplyModifiedProperties();
			}
		}
#endif
	}
}

