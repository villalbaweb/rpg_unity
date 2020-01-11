using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour 
    {
        // config params
        [SerializeField] float healthPoints = 100f;

        // cache
        Animator _animator;

        // state
        bool isDead;

        private void Start() {
            _animator = GetComponent<Animator>();

            isDead = false;
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if (!isDead && healthPoints == 0) {
                _animator.SetTrigger("Die");
                isDead = true;
            }
        }
    }
}
