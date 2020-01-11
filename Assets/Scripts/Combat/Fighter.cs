using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{

    public class Fighter : MonoBehaviour, IAction 
    {
        // config params
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 5;

        // cache
        Mover _mover;
        ActionScheduler _actionScheduler;
        Animator _animator;

        // state
        Transform target;
        float timeSinceLastAttack = 0;

        private void Start() 
        {
            _mover = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _animator = GetComponent<Animator>();
        }

        private void Update() 
        {
            timeSinceLastAttack += Time.deltaTime;

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
            if (timeSinceLastAttack >= timeBetweenAttacks){
                timeSinceLastAttack = 0;
                _animator.SetTrigger("Attack");
            }
        }

        // Animation Event
        private void Hit()
        {
            target.gameObject.GetComponent<Health>().TakeDamage(weaponDamage);
        }
    }

}