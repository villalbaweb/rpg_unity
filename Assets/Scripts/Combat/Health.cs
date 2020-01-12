using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour 
    {
        // config params
        [SerializeField] float healthPoints = 100f;

        // cache
        Animator _animator;

        // properties
        public bool IsDead { get; set;}

        private void Start() {
            _animator = GetComponent<Animator>();

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
            }
        }
    }
}
