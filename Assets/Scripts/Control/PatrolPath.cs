using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control 
{
    public class PatrolPath : MonoBehaviour
    {
        const float gizmoSize = 0.3f;

        private void OnDrawGizmos() {
            for (int i = 0; i < transform.childCount; i++)
            {
                Vector3 gizmoPosition = GetWaypoint(i);
                Vector3 nextGizmoPosition = GetWaypoint(i + 1);

                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(gizmoPosition, gizmoSize);
                Gizmos.DrawLine(gizmoPosition, nextGizmoPosition);
            }
        }

        private Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i == transform.childCount ? 0 : i).position;
        }
    }
}
