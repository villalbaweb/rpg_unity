using RPG.Core;
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
        ActionScheduler _actionScheduler;

        // state
        Transform target;

        private void Start() 
        {
            _mover = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Update() 
        {
            MoveToAttackPoint();
        }

        public void Attack(CombatTarget combatTarget)
        {
            _actionScheduler.StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
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