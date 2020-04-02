using RPG.Saving;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        // config params
        [SerializeField] float healthPoints = 100f;

        // cache
        Animator _animator;
        ActionScheduler _actionScheduler;

        // state
        bool isDead;

        private void Start() {
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();

            isDead = false;
        }

        public bool IsDead()
        {
            return healthPoints == 0;
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            DieHandler();
        }

        private void DieHandler()
        {
            if (!isDead && healthPoints == 0)
            {
                _animator.SetTrigger("Die");
                isDead = true;
                _actionScheduler.CancelCurrentAction();
            }
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
