using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour 
    {
        // config params
        [SerializeField] float healthPoints = 100f;

        // cache
        Animator _animator;
        ActionScheduler _actionScheduler;

        // properties
        public bool IsDead { get; set;}

        private void Start() {
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();

            IsDead = false;
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            DieHandler();
        }

        private void DieHandler()
        {
            if (!IsDead && healthPoints == 0)
            {
                _animator.SetTrigger("Die");
                IsDead = true;
                _actionScheduler.CancelCurrentAction();
            }
        }
    }
}