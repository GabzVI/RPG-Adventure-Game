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

		[SerializeField] Transform target;
		[SerializeField] float maxSpeed = 6.0f;

		NavMeshAgent navmeshAgent;
		Health health;
		private void Start()
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

		public void StartMoving(Vector3 destination, float speedFraction)
		{
			GetComponent<ActionScheduler>().StartAction(this);
			MoveTo(destination, speedFraction);
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

