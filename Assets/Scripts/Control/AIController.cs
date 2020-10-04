using GameDevTV.Utils;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
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
        [SerializeField] float aggervateTime = 5f;

        // cache
        Fighter _fighter;
        Health _health;
        GameObject _player;
        Mover _mover;
        ActionScheduler _actionScheduler;
        Rigidbody _rigidBody;

        // state
        LazyValue<Vector3> guardLocation;
        float timeSinceLastSawPLayer = Mathf.Infinity;
        float timeSinceReachWaypoint = Mathf.Infinity;
        float timeSinceAggrevation = Mathf.Infinity;
        int patrolPathWaypointIndex = 0;

        private void Awake()
        {
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _player = GameObject.FindWithTag("Player");
            _mover = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _rigidBody = GetComponent<Rigidbody>();

            guardLocation = new LazyValue<Vector3>(GuardLocationInitializer);
        }

        private Vector3 GuardLocationInitializer()
        {
            return transform.position;
        }

        private void Update()
        {
            if (_health.IsDead())
            {
                Destroy(_rigidBody); //destroy this component in order to avoid strange interactions
                return;
            }

            Chase();

            UpdateTimers();
        }

        public void Aggrevate()
        {
            timeSinceAggrevation = 0;
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPLayer += Time.deltaTime;
            timeSinceReachWaypoint += Time.deltaTime;
            timeSinceAggrevation += Time.deltaTime;
        }

        private void Chase()
        {
            if ((IsAggreviated() && _fighter.CanAttack(_player)))
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
            Vector3 nextPosition = guardLocation.value;

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

        private bool IsAggreviated() {

            bool isInAttackRange = Vector3.Distance(_player.transform.position, transform.position) <= chaseDistance;
            bool isAggreviated = timeSinceAggrevation < aggervateTime;
            return isInAttackRange || isAggreviated;
        }

        // called by Unity
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
