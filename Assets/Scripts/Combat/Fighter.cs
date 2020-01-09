using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{

    public class Fighter : MonoBehaviour 
    {
        // config params
        [SerializeField] float weaponRange = 2f;

        // cache
        Mover _mover;

        // state
        Transform target;

        private void Start() 
        {
            _mover = GetComponent<Mover>();
        }

        private void Update() 
        {
            MoveToAttackPoint();
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;
        }
        
        private void MoveToAttackPoint()
        {
            if (target == null) return;

            bool isInRange = Vector3.Distance(target.position, transform.position) <= weaponRange;
            if (!isInRange)
            {
                _mover.MoveTo(target.position);
            } 
            else
            {
                _mover.Stop();
            }
        }
    }

}