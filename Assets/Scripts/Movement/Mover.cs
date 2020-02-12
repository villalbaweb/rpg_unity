using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;

namespace RPG.Movement
{

    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        // cache
        NavMeshAgent _agent;
        Animator _animator;
        ActionScheduler _actionScheduler;
        Health _health;

        // Start is called before the first frame update
        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            _agent.enabled = !_health.IsDead;

            UpdateAnimator();
        }

        public void Cancel()
        {
            _agent.isStopped = true;
        }

        public void StartMoveAction(Vector3 destination)
        {
            _actionScheduler.StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 newPosition)
        {
            _agent.isStopped = false;
            _agent.destination = newPosition;
        }

        public void SetMovementSpeed(float speed)
        {
            _agent.speed = speed;
        }

        private void UpdateAnimator() 
        {
            Vector3 localVelocity = transform.InverseTransformDirection(_agent.velocity);
            float speed = localVelocity.z;
            _animator.SetFloat("ForwardSpeed", speed);
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 positon = state as SerializableVector3;
            _agent.enabled = false;
            transform.position = positon.ToVector();
            _agent.enabled = true;
        }
    }

}