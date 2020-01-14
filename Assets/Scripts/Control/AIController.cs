using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        // config params
        [SerializeField] float chaseDistance = 5f;

        private void Update()
        {
            Chase();
        }

        private void Chase()
        {
            if (DistanceToPlayer() <= chaseDistance)
            {
                print($"{gameObject.name} in range to chase...");
            }
        }

        private float DistanceToPlayer() {
            GameObject player = GameObject.FindWithTag("Player");
            return Vector3.Distance(player.transform.position, transform.position);
        }
    }
}
