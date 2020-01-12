using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour {
        
        // cache
        Mover _mover;
        Fighter _fighter;
        
        private void Start() 
        {
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
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

                if(!_fighter.CanAttack(target)) continue;

                if(Input.GetMouseButtonDown(0))
                {
                    _fighter.Attack(target);
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