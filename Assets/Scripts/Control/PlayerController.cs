using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using RPG.Movement;
using System;
using RPG.Combat;
using RPG.Core;
using RPG.Resources;
using UnityEngine.AI;


namespace RPG.Control
{
	public class PlayerController : MonoBehaviour
	{
		Health health;

		[System.Serializable]
		struct CursorMapping
		{
			public CursorType type;
			public Texture2D texture;
			public Vector2 hotspot;
		}

		//An array of different cursor mapping which will be used later on to pick the cursor we want.
		[SerializeField] CursorMapping[] cursorMappings = null;
		[SerializeField] float maxDistanceNavProjection = 1f;
		[SerializeField] float maxPathLength = 35f;
		[SerializeField] float raycastRadius = 1f;

		private void Start()
		{
			health = GetComponent<Health>();
			//Cursor.lockState = CursorLockMode.Confined;
		}

		// Update is called once per frame
		void Update()
		{
			if (InteractableWithUI()) { return; }
			//If player is dead will disable the raycast component and movement.
			if (health.IsDead())
			{
				SetCursor(CursorType.None);
				return;
			}

			if(InteractWithComponent()) { return; }
			if (InteractWithMovement()) { return; }
			SetCursor(CursorType.None);
		}

		private bool InteractWithComponent()
		{
			//This is done so that the ray penetrates any object and targets/detects enemies as a priority.
			RaycastHit[] hits = RaycastAllSorted();

			//This will check for all the objects the ray cast hit and store it inside hitInfo.
			foreach (RaycastHit hitInfo in hits)
			{
				//This loop will check for all raycasted objects in a list and check whether or not a action can be one or they are raycastable.
				IRaycastable[] raycastables = hitInfo.transform.GetComponents<IRaycastable>();
				foreach(IRaycastable raycastable in raycastables)
				{
					if (raycastable.HandleRaycast(this))
					{
						SetCursor(raycastable.GetCursorType());
						return true;
					}
				}
			}
			//If all the objects hit are non raycastable just return false/
			return false;
		}

		private bool InteractableWithUI()
		{
			//IspointerOverGameobject checks for the gameobject that is of UI type. 
			if (EventSystem.current.IsPointerOverGameObject())
			{
				SetCursor(CursorType.UI);
				return true;
			}
			return false;
		}

		RaycastHit[] RaycastAllSorted()
		{
			RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);

			//contains the distances between player and character
			float[] distances = new float[hits.Length];

			for(int i = 0; i < hits.Length; i++)
			{
				//Allocating the distance of each object hit by ray cast into the array of distances.
				distances[i] = hits[i].distance;
			}

			Array.Sort(distances, hits);
			return hits;
		}

		private bool InteractWithMovement()
		{
			Vector3 target;
			bool hasHit = RaycastNavMesh(out target);

			if (hasHit)
			{
				if (Input.GetMouseButton(1))
				{
					//1f is to ensure player moves at full speed
					GetComponent<Mover>().StartMoving(target, 1f);
				}
				SetCursor(CursorType.Movement);
				return true;
			}
			return false;
		}

		//This function will be used to prevent players walking in areas in the navmesh that arent walkable.
		private bool RaycastNavMesh(out Vector3 target)
		{
			target = new Vector3();
			//Stores the ray thats being shot from camera and stores whats it hitting into hitInfo.
			RaycastHit hitInfo;
			bool hasHit = Physics.Raycast(GetMouseRay(), out hitInfo);

	
			if (!hasHit) { return false; }
			//navMeshHit Stores the posiiton where raycast has been cast to.
			NavMeshHit navMeshHit;
			bool hasCastToNavMesh  = NavMesh.SamplePosition(hitInfo.point, out navMeshHit, maxDistanceNavProjection, NavMesh.AllAreas);
			if (!hasCastToNavMesh) { return false; }

			target = navMeshHit.position;

			//reference types can be null, so we need to give it a navmeshpath to ensure it isnt null.
			NavMeshPath path = new NavMeshPath();
			bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
			if (!hasPath) { return false; }

			//Checks whether or not the path is complete, and returns false if not complete
			if (path.status != NavMeshPathStatus.PathComplete) { return false; }
			if(GetPathLengh(path) > maxPathLength) { return false; }

			return true;
		}

		private float GetPathLengh(NavMeshPath path)
		{
			float total = 0;

			if (path.corners.Length < 2) { return total; }

			for (int i = 0; i < path.corners.Length - 1; i++)
			{
				//returns the distance between the first vector point in the corners and the next vector point of the corners.
				total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
			}

			return total;
			
		}

		private void SetCursor(CursorType type)
		{
			//Using unity cursor to configure our cursor.
			CursorMapping mapping = GetCursorMapping(type);
			Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
		}

		private CursorMapping GetCursorMapping(CursorType type)
		{
			foreach(CursorMapping mapping in cursorMappings)
			{
				if(mapping.type == type)
				{
					return mapping;
				}
			}
			return cursorMappings[0];
		}

		private static Ray GetMouseRay()
		{
			return Camera.main.ScreenPointToRay(Input.mousePosition);
		}

	}
}

