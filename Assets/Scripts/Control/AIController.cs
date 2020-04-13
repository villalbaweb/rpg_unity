﻿using System;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        // config params
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspitionTime = 5f;
        [SerializeField] float waypointWaitTime = 2f;
        [SerializeField] PatrolPath patrolPath = null;
        [SerializeField] float waypointTolerance = 1f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.5f;
        [SerializeField] float maxSpeed = 5f;

        // cache
        Fighter _fighter;
        Health _health;
        GameObject _player;
        Mover _mover;
        ActionScheduler _actionScheduler;
        Rigidbody _rigidBody;

        // state
        Vector3 guardLocation;
        float timeSinceLastSawPLayer = Mathf.Infinity;
        float timeSinceReachWaypoint = Mathf.Infinity;
        int patrolPathWaypointIndex = 0;

        private void Start() {
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _player = GameObject.FindWithTag("Player");
            _mover = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _rigidBody = GetComponent<Rigidbody>();

            guardLocation = transform.position;
        }

        private void Update()
        {
            if (_health.IsDead()) 
            {
                Destroy(_rigidBody); //destroy this component in order to avoid strange interactions
                return;
            }

            timeSinceLastSawPLayer += Time.deltaTime;
            timeSinceReachWaypoint += Time.deltaTime;
            
            Chase();
        }

        private void Chase()
        {
            if (IsInAttackRange() && _fighter.CanAttack(_player))
            {
                AttackBehavior();
            }
            else if (timeSinceLastSawPLayer <= suspitionTime)
            {
                SuspiciousBehavior();
            }
            else
            {
                PatrolBehavior();
            }
        }

        private void PatrolBehavior()
        {
            Vector3 nextPosition = guardLocation;

            if (patrolPath != null){
                if (AtWaypoint()) {
                    timeSinceReachWaypoint = 0;
                    patrolPathWaypointIndex = CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint(patrolPathWaypointIndex);
            }

            if(timeSinceReachWaypoint > waypointWaitTime) {
                _mover.SetMovementSpeed(maxSpeed * patrolSpeedFraction);
                _mover.StartMoveAction(nextPosition);
            }
        }

        private Vector3 GetCurrentWaypoint(int index)
        {
            return patrolPath.GetWaypoint(index);
        }

        private int CycleWaypoint()
        {
            return patrolPath.NextPosition(patrolPathWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint(patrolPathWaypointIndex));
            return distanceToWaypoint < waypointTolerance;
        }

        private void SuspiciousBehavior()
        {
            _actionScheduler.CancelCurrentAction();
        }

        private void AttackBehavior()
        {
            timeSinceLastSawPLayer = 0;
            _mover.SetMovementSpeed(maxSpeed);
            _fighter.Attack(_player);
        }

        private bool IsInAttackRange() {
            return Vector3.Distance(_player.transform.position, transform.position) <= chaseDistance;
        }

        // called by Unity
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
