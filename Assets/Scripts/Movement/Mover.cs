using RPG.Core;
using RPG.Combat;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{

    public class Mover : MonoBehaviour
    {
        // cache
        NavMeshAgent _agent;
        Animator _animator;
        ActionScheduler _actionScheduler;
        Fighter _fighter;

        // Start is called before the first frame update
        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _fighter = GetComponent<Fighter>(); // circular dependency ????
        }

        // Update is called once per frame
        void Update()
        {
            UpdateAnimator();
        }

        public void Stop()
        {
            _agent.isStopped = true;
        }

        public void StartMoveAction(Vector3 destination)
        {
            _actionScheduler.StartAction(this);
            _fighter.Cancel();
            MoveTo(destination);
        }

        public void MoveTo(Vector3 newPosition)
        {
            _agent.isStopped = false;
            _agent.destination = newPosition;
        }

        private void UpdateAnimator() 
        {
            Vector3 localVelocity = transform.InverseTransformDirection(_agent.velocity);
            float speed = localVelocity.z;
            _animator.SetFloat("ForwardSpeed", speed);
        }
    }

}