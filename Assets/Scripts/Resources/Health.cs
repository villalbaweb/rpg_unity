using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        // config params
        [SerializeField] float healthPoints = 100f;
        [SerializeField] float regenerationPercentage = 70;

        // cache
        Animator _animator;
        ActionScheduler _actionScheduler;
        BaseStats _baseStats;

        // state
        bool isDead = false;

        private void Start() {
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _baseStats = GetComponent<BaseStats>();

            _baseStats.OnLevelUpEvent += RegenerateHealth;

            healthPoints = isDead ? 0 : _baseStats.GetStat(Stat.Health);
        }

        public bool IsDead()
        {
            return healthPoints == 0;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            DieHandler(instigator);
        }

        private void DieHandler(GameObject instigator)
        {
            if (!isDead && healthPoints == 0)
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
            return (healthPoints * 100 ) / _baseStats.GetStat(Stat.Health);
        }

        private void RegenerateHealth()
        {
            float regenerateHealt = _baseStats.GetStat(Stat.Health) * regenerationPercentage / 100;
            healthPoints = Mathf.Max(healthPoints , regenerateHealt);
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            UpdateAfterRestore();

        }

        private void UpdateAfterRestore()
        {
            isDead = healthPoints == 0;
            
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
