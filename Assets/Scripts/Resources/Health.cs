using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        // config params
        [SerializeField] float regenerationPercentage = 70;

        // cache
        Animator _animator;
        ActionScheduler _actionScheduler;
        BaseStats _baseStats;

        // state
        bool isDead = false;
        LazyValue<float> healthPoints;

        // porperties
        public float HealthPoints => healthPoints.value;
        public float MaxHealthPoints => _baseStats.GetStat(Stat.Health);

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _baseStats = GetComponent<BaseStats>();

            healthPoints = new LazyValue<float>(HealthPointsInitializer);
        }

        private float HealthPointsInitializer()
        {
            return isDead ? 0 : _baseStats.GetStat(Stat.Health);
        }

        private void OnEnable()
        {
            if(_baseStats)
            {
                _baseStats.OnLevelUpEvent += RegenerateHealth;
            }
        }

        private void OnDisable()
        {
            if(_baseStats)
            {
                _baseStats.OnLevelUpEvent -= RegenerateHealth;
            }
        }

        public bool IsDead()
        {
            return healthPoints.value == 0;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);

            DieHandler(instigator);
        }

        private void DieHandler(GameObject instigator)
        {
            if (!isDead && healthPoints.value == 0)
            {
                _animator.SetTrigger("Die");
                isDead = true;
                _actionScheduler.CancelCurrentAction();
                AwardExperience(instigator);
            }
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if(!experience) return;

            experience.GainExperience(_baseStats.GetStat(Stat.ExperienceReward));
        }

        public float GetPercentage()
        {
            return (healthPoints.value * 100 ) / _baseStats.GetStat(Stat.Health);
        }

        private void RegenerateHealth()
        {
            float regenerateHealt = _baseStats.GetStat(Stat.Health) * regenerationPercentage / 100;
            healthPoints.value = Mathf.Max(healthPoints.value , regenerateHealt);
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;
            UpdateAfterRestore();

        }

        private void UpdateAfterRestore()
        {
            isDead = healthPoints.value == 0;
            
            _animator = GetComponent<Animator>();

            if(isDead) {
                _animator.SetTrigger("Die");
            } else {
                _animator.SetTrigger("AliveAfterReload");

                // this is required because after load the saved game if the enemy is alive it when it dies, the AliveAFterReload trigger remains set
                // and pass the animation to locomotion blend tree
                _animator.ResetTrigger("AliveAfterReload"); 
            } 
        }
    }
}
