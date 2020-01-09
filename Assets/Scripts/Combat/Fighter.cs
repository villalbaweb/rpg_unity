using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{

    public class Fighter : MonoBehaviour, IAction 
    {
        // config params
        [SerializeField] float weaponRange = 2f;

        // cache
        Mover _mover;
        ActionScheduler _actionScheduler;
        Animator _animator;

        // state
        Transform target;

        private void Start() 
        {
            _mover = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _animator = GetComponent<Animator>();
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
                _mover.Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            _animator.SetTrigger("Attack");
        }

        // Animation Event
        private void Hit()
        {
            print("Punching...");
        }
    }

}