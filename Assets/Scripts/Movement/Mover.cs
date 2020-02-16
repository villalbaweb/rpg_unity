using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using System.Collections.Generic;

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
            _agent.enabled = !_health.IsDead();

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
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("position", new SerializableVector3(transform.position));
            data.Add("rotation", new SerializableVector3(transform.eulerAngles));
            return data;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> data = state as Dictionary<string, object>;

            SerializableVector3 positon = data["position"] as SerializableVector3;
            SerializableVector3 rotation = data["rotation"] as SerializableVector3;
            _agent = GetComponent<NavMeshAgent>();
            _agent.enabled = false;
            transform.position = positon.ToVector();
            transform.eulerAngles = rotation.ToVector();
            _agent.enabled = true;
        }
    }

}