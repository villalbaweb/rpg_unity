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
        Health target;
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

        public bool CanAttack(CombatTarget combatTarget)
        {
            if (combatTarget == null) return false;

            Health posibleTarget = combatTarget.GetComponent<Health>();
            return posibleTarget != null && !posibleTarget.IsDead;
        }

        public void Attack(CombatTarget combatTarget)
        {
            _actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            target = null;
            _animator.SetTrigger("Stop Attack");
        }
        
        private void MoveToAttackPoint()
        {
            if (target == null) return;

            bool isInRange = Vector3.Distance(target.transform.position, transform.position) <= weaponRange;
            if (!isInRange)
            {
                _mover.MoveTo(target.transform.position);
            } 
            else
            {
                _mover.Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            if (target.IsDead) return;

            transform.LookAt(target.transform);

            if (timeSinceLastAttack >= timeBetweenAttacks){
                timeSinceLastAttack = 0;
                _animator.SetTrigger("Attack");
            }
        }

        // Animation Event
        private void Hit()
        {
            if (target == null) return;

            target.TakeDamage(weaponDamage);
        }
    }

}