using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using RPG.Resources;
using System;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour {
        
        // configs
        [SerializeField] CursorMapping[] cursorMappings = null;

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

            if(InteractWithCombat()) return;
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

        private bool InteractWithCombat()
        {
            RaycastHit[] hitsData = Physics.RaycastAll(GetMouseRay());
            
            foreach(RaycastHit hit in hitsData)
            {
                CombatTarget target = hit.collider.GetComponent<CombatTarget>();

                if(target == null || !_fighter.CanAttack(target.gameObject)) continue;

                if(Input.GetMouseButton(0))
                {
                    _fighter.Attack(target.gameObject);
                }

                SetCursor(CursorType.Combat);

                return true;
            }
            
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hitData;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hitData);
            Vector3 newPosition = hasHit ? hitData.point : transform.position;
            if (Input.GetMouseButton(0))
            {
                _mover.StartMoveAction(newPosition);
            }

            SetCursor(CursorType.Movement);

            return hasHit;
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