using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{

    public class Fighter : MonoBehaviour, IAction 
    {
        // config params
        
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform handTransform = null;
        [SerializeField] Weapon weapon = null;

        // cache
        Mover _mover;
        ActionScheduler _actionScheduler;
        Animator _animator;

        // state
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;

        private void Start() 
        {
            _mover = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _animator = GetComponent<Animator>();

            SpawnWeapon();
        }

        private void Update() 
        {
            timeSinceLastAttack += Time.deltaTime;

            MoveToAttackPoint();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;

            Health posibleTarget = combatTarget.GetComponent<Health>();
            return posibleTarget != null && !posibleTarget.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            _actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            target = null;
            _mover.Cancel();
            TriggerStopAttack();
        }

        private void SpawnWeapon()
        {
            if(!weapon) return;

            Animator _animator = GetComponent<Animator>();
            weapon.Spawn(handTransform, _animator);
        }

        private void MoveToAttackPoint()
        {
            if (target == null) return;

            bool isInRange = Vector3.Distance(target.transform.position, transform.position) <= weapon.WeaponRange;
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
            if (target.IsDead()) return;

            transform.LookAt(target.transform);

            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                timeSinceLastAttack = 0;
                TriggerAttack();
            }
        }

        private void TriggerAttack()
        {
            _animator.ResetTrigger("Stop Attack");
            _animator.SetTrigger("Attack");
        }

        private void TriggerStopAttack()
        {
            _animator.ResetTrigger("Attack");
            _animator.SetTrigger("Stop Attack");
        }

        // Animation Event
        private void Hit()
        {
            if (target == null) return;

            target.TakeDamage(weapon.WeaponDamage);
        }
    }

}