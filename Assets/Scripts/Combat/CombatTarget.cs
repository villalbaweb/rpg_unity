using RPG.Control;
using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRayCastable
    {
        public bool HandleRaycast(PlayerController callingController)
        {
            Fighter _fighter = callingController.GetComponent<Fighter>();

            if (!_fighter.CanAttack(gameObject)) return false;

            if (Input.GetMouseButton(0))
            {
                _fighter.Attack(gameObject);
            }

            return true;
        }
    }
}