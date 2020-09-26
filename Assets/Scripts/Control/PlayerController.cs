using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using RPG.Attributes;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour {
        
        // configs
        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavMeshProjectionDistance = 1f;
        [SerializeField] float maxPathLenght = 10;

        // cache
        Mover _mover;
        Fighter _fighter;
        Health _health;
        Rigidbody _rigidBody;
        
        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if(InteractWithUI()) return;

            if (_health.IsDead())
            {
                SetCursor(CursorType.None);
                Destroy(_rigidBody); //destroy this component in order to avoid strange interactions
                return;
            }

            if(InteractWithComponent()) return;
            if(InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithUI()
        {
            bool result = false;
            if(EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                result = true;
            }

            return result;
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hitsData = RaycastAllSorted();

            foreach (RaycastHit hit in hitsData)
            {
                IRayCastable[] rayCastables = hit.transform.GetComponents<IRayCastable>();

                foreach(IRayCastable rayCastable in rayCastables)
                {
                    if(rayCastable.HandleRaycast(this))
                    {
                        SetCursor(rayCastable.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }

        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            float[] distances = new float[hits.Length];

            for(int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            Array.Sort(distances, hits);

            return hits;
        }

        private bool InteractWithMovement()
        {
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);

            if (Input.GetMouseButton(0))
            {
                _mover.StartMoveAction(target);
            }

            SetCursor(CursorType.Movement);

            return hasHit;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();

            RaycastHit hitData;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hitData);
            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hitData.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);

            if (!hasCastToNavMesh) return false;

            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, navMeshHit.position, NavMesh.AllAreas, path);

            if (IsBadPath(path, hasPath)) return false;

            target = navMeshHit.position;
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

            if(path.corners.Length < 2) return result;

            for(int i = 0; i < path.corners.Length - 1; i++)
            {
                result += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return result;
        }

        private void SetCursor(CursorType cursorType)
        {
            CursorMapping mapping = GetCursorMapping(cursorType);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach(CursorMapping mapping in cursorMappings)
            {
                if(mapping.type == type)
                {
                    return mapping;
                }
            }

            return cursorMappings[0];
        }

        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}