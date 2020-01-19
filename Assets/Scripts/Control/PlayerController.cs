using RPG.Combat;
using RPG.Movement;
using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour {
        
        // cache
        Mover _mover;
        Fighter _fighter;
        Health _health;
        
        private void Start() 
        {
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            if (_health.IsDead) return;

            if(InteractWithCombat()) return;
            if(InteractWithMovement()) return;
            print("Notingh to do there...");
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

            return hasHit;
        }

        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}