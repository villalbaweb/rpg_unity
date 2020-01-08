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
            InteractWithMovement();
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hitsData = Physics.RaycastAll(GetMouseRay());
            
            foreach(RaycastHit hit in hitsData)
            {
                CombatTarget target = hit.collider.GetComponent<CombatTarget>();

                if (target == null) continue;

                if(Input.GetMouseButtonDown(0))
                {
                    _fighter.Attack(target);
                }

                return true;
            }
            
            return false;
        }

        private void InteractWithMovement()
        {
            if (Input.GetMouseButton(0))
            {
                MoveToCursor();
            }
        }

        private void MoveToCursor()
        {
            RaycastHit hitData;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hitData);
            Vector3 newPosition = hasHit ? hitData.point : transform.position;
            _mover.MoveTo(newPosition);
        }

        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}