using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Core;
using RPG.Movement;
using RPG.Resources;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{

    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        // config params
        
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;

        // cache
        Mover _mover;
        ActionScheduler _actionScheduler;
        Animator _animator;
        BaseStats _baseStats;

        // state
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        LazyValue<Weapon> currentWeapon;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _animator = GetComponent<Animator>();
            _baseStats = GetComponent<BaseStats>();

            currentWeapon = new LazyValue<Weapon>(CurrentWeaponInitialize);
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        private Weapon CurrentWeaponInitialize()
        {
            AttachWeapon(defaultWeapon);
            return defaultWeapon;
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

        public void EquipWeapon(Weapon weapon)
        {
            if(!weapon) return;

            currentWeapon.value = weapon;

            AttachWeapon(weapon);
        }

        private void AttachWeapon(Weapon weapon)
        {
            Animator _animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, _animator);
        }

        public Health GetTarget()
        {
            return target;
        }

        private void MoveToAttackPoint()
        {
            if (target == null) return;

            bool isInRange = Vector3.Distance(target.transform.position, transform.position) <= currentWeapon.value.WeaponRange;
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

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return currentWeapon.value.WeaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return currentWeapon.value.PercentageBonus;
            }
        }

        // Animation Events
        private void Hit()
        {
            if (target == null) return;

            float damage = _baseStats.GetStat(Stat.Damage);

            if (currentWeapon.value.HasProjectile)
            {
                currentWeapon.value.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }

        }

        private void Shoot()
        {
            Hit();
        }

        public object CaptureState()
        {
            return currentWeapon.value.name;
        }

        public void RestoreState(object state)
        {
            string restoredWeaponName = state as string;

            // Resources is a special folder that will include all the assets and refs to the build
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(restoredWeaponName);
            EquipWeapon(weapon);
        }
    }

}