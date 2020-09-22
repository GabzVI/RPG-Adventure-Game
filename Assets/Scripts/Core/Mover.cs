using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Resources;


namespace RPG.Movement
{
	public class Mover : MonoBehaviour, IAction
	{

		[SerializeField] float maxSpeed = 6.0f;
		[SerializeField] float maxPathLength = 35f;
		


		NavMeshAgent navmeshAgent;
		Health health;
		

		private void Awake()
		{
			navmeshAgent = GetComponent<NavMeshAgent>();
			health = GetComponent<Health>();
		}
	
		// Update is called once per frame
		void Update()
		{
			navmeshAgent.enabled = !health.IsDead();

			UpdateAnimator();
		}

		public bool CanMoveTo(Vector3 destination)
		{
			//reference types can be null, so we need to give it a navmeshpath to ensure it isnt null.
			NavMeshPath path = new NavMeshPath();
			bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
			if (!hasPath) { return false; }
			//Checks whether or not the path is complete, and returns false if not complete
			if (path.status != NavMeshPathStatus.PathComplete) { return false; }
			if (GetPathLengh(path) > maxPathLength) { return false; }

			return true;
		}

		
		public void StartMoving(Vector3 destination, float speedFraction)
		{
			GetComponent<ActionScheduler>().StartAction(this);
			MoveTo(destination, speedFraction);
			navmeshAgent.isStopped = false;
		}

		public void MoveTo(Vector3 destination, float speedFraction)
		{
			navmeshAgent.destination = destination;
		    navmeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
			navmeshAgent.isStopped = false;

		}

		public void Cancel()
		{
			navmeshAgent.isStopped = true;
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
		private void UpdateAnimator()
		{
			Vector3 velocity = navmeshAgent.velocity;
			//This changes from coordinates from world/global space to local space. This is so the animator is able to know at which speed the object is moving in local space in order to activate animation.
			Vector3 localVelocity = transform.InverseTransformDirection(velocity);

			//Belowe we set a float which will be used by the animator to determine which animation to trigger by getting the local velocity in the z-axis that the object is moving in.
			float speed = localVelocity.z;
			GetComponent<Animator>().SetFloat("forwardSpeed", speed);

		}

		
	}
}

