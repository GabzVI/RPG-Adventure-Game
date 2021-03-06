﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Resources;
using System;
using GameDevTV.Utils;

namespace RPG.Control
{
	public class AIController : MonoBehaviour
	{
		[SerializeField] float chaseDistance = 8f;
		[SerializeField] float suspicionTime = 3.0f;
		[SerializeField] float aggroCD = 5.0f;
		[SerializeField] ControlPath patrolPath;
		[SerializeField] float waypointTolerance = 1.0f;
		[SerializeField] float dwellTime = 3.0f;
		[SerializeField] float helpDistance = 8f;
		[Range(0,1)]
		[SerializeField] float patrolSpeedFraction = 0.2f;
	
		FighterScript fighter;
		Health health;
		GameObject player;
		Mover mover;

		LazyValue<Vector3> guardPosition;

		float timeSincePlayerSeen = Mathf.Infinity;
		float timeSinceArriveWaypoint = Mathf.Infinity;
		float timeSinceAggrevated = Mathf.Infinity;
		int currentWayPointIndex = 0;


		private void Awake()
		{
			
			fighter = GetComponent<FighterScript>();
			player = GameObject.FindWithTag("Player");
			health = GetComponent<Health>();
			mover = GetComponent<Mover>();
			guardPosition = new LazyValue<Vector3>(GetGuardPosition);
		}
		// Start is called before the first frame update
		void Start()
		{
			guardPosition.ForceInit();
		}

		private Vector3 GetGuardPosition()
		{
			return transform.position;
		}
		// Update is called once per frame
		void Update()
		{ 
			if (health.IsDead()) { return; }

			if (IsAggrevated() && fighter.CanAttack(player))
			{
				AttackBehaviour();
			}
			else if (timeSincePlayerSeen < suspicionTime )
			{
				//Suspicion State
				SuspiciousBehaviour();
			}
			else
			{
				PatrolBehaviour();
			}

			UpdateTimers();
		}

		public void Aggrevate()
		{
			if (ToFartoChase())
			{
				timeSinceAggrevated = 0;
			}
		}

		private void UpdateTimers()
		{
			timeSincePlayerSeen += Time.deltaTime;
			timeSinceArriveWaypoint += Time.deltaTime;
			timeSinceAggrevated += Time.deltaTime;
		}

		private void SuspiciousBehaviour()
		{
			GetComponent<ActionScheduler>().CancelCurrentAction();
		}

		private void PatrolBehaviour()
		{
			Vector3 nextPos = guardPosition.value;

			if(patrolPath != null)
			{
				if (AtWaypoint())
				{
					timeSinceArriveWaypoint = 0.0f;
					CycleWaypoint();
				}
				nextPos = GetCurrentWaypoint();
			}
		   
			if(timeSinceArriveWaypoint > dwellTime)
			{
				mover.StartMoving(nextPos, patrolSpeedFraction);
			}
		   
		}

		private bool AtWaypoint()
		{
			float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
			return distanceToWaypoint < waypointTolerance;
		}

		private Vector3 GetCurrentWaypoint()
		{
			return patrolPath.GetWayPointPos(currentWayPointIndex);
		}

		private void CycleWaypoint()
		{
			currentWayPointIndex = patrolPath.GetNextIndex(currentWayPointIndex);
		}

		private void AttackBehaviour()
		{
			timeSincePlayerSeen = 0.0f;
			fighter.Attack(player);

			AggrevateNearbyEnemies();
		}

		private void AggrevateNearbyEnemies()
		{
			RaycastHit[] hits = Physics.SphereCastAll(transform.position, helpDistance, Vector3.up, 0);

			foreach(RaycastHit hit in hits)
			{
				AIController ai = hit.collider.GetComponent<AIController>();
				if(ai == null) { continue; }
				ai.Aggrevate();
			}
		}

		private bool IsAggrevated()
		{
			float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
			return distanceToPlayer < chaseDistance || timeSinceAggrevated < aggroCD;
		}

		private bool ToFartoChase()
		{
			float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
			if (distanceToPlayer >= chaseDistance) { return true; } else { return false; }
			
		}

		//Called by Unity
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, chaseDistance);
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, helpDistance);
		}

	}
}

