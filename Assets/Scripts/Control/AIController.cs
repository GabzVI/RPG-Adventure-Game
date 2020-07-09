using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5.0f;
        [SerializeField] float suspicionTime = 5.0f;
        [SerializeField] ControlPath patrolPath;
        [SerializeField] float waypointTolerance = 1.0f;
        [SerializeField] float dwellTime = 3.0f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;

        FighterScript fighter;
        Health health;
        GameObject player;
        Mover mover;

        Vector3 guardPosition;

        float timeSincePlayerSeen = Mathf.Infinity;
        float timeSinceArriveWaypoint = Mathf.Infinity;
        int currentWayPointIndex = 0;

  
        // Start is called before the first frame update
        void Start()
        {
            fighter = GetComponent<FighterScript>();
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();

            guardPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (health.IsDead()) { return; }

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (timeSincePlayerSeen < suspicionTime)
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

        private void UpdateTimers()
        {
            timeSincePlayerSeen += Time.deltaTime;
            timeSinceArriveWaypoint += Time.deltaTime;
        }

        private void SuspiciousBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPos = guardPosition;

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
        }

        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }

        //Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}

