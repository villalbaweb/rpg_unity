using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control 
{
    public class PatrolPath : MonoBehaviour
    {
        const float gizmoSize = 0.3f;

        private void OnDrawGizmosSelected() {
            for (int i = 0; i < transform.childCount; i++)
            {
                Vector3 gizmoPosition = transform.GetChild(i).position;

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(gizmoPosition, gizmoSize);
            }
        }
    }
}
