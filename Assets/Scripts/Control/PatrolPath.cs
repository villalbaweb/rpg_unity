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
                Vector3 nextGizmoPosition = GetWaypoint(NextPosition(i));

                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(gizmoPosition, gizmoSize);
                Gizmos.DrawLine(gizmoPosition, nextGizmoPosition);
            }
        }

        public int NextPosition(int i)
        {
            return i == transform.childCount - 1 
                ? 0 
                : i + 1;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
