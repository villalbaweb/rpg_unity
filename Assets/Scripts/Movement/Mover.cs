﻿using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{

    public class Mover : MonoBehaviour
    {
        // cache
        NavMeshAgent _agent;
        Animator _animator;

        // Start is called before the first frame update
        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
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