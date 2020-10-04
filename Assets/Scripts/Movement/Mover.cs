using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using System.Collections.Generic;
using RPG.Attributes;

namespace RPG.Movement
{

    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float maxPathLenght = 10;

        // cache
        NavMeshAgent _agent;
        Animator _animator;
        ActionScheduler _actionScheduler;
        Health _health;

        private void Awake()
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

        public bool CanMoveTo(Vector3 targetPos)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, targetPos, NavMesh.AllAreas, path);

            if (IsBadPath(path, hasPath)) return false;

            return true;
        }

        private bool IsBadPath(NavMeshPath path, bool hasPath)
        {
            return !hasPath ||
                    path.status != NavMeshPathStatus.PathComplete ||
                    GetPathLength(path) > maxPathLenght;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float result = 0;

            if (path.corners.Length < 2) return result;

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                result += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return result;
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