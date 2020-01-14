using RPG.Combat;
using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        // config params
        [SerializeField] float chaseDistance = 5f;

        // cache
        Fighter _fighter;
        Health _health;
        GameObject _player;

        private void Start() {
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _player = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
            if (_health.IsDead) return;
            
            Chase();
        }

        private void Chase()
        {
            if (IsInAttackRange() && _fighter.CanAttack(_player))
            {
                _fighter.Attack(_player);
            }
            else 
            {
                _fighter.Cancel();
            }
        }

        private bool IsInAttackRange() {
            return Vector3.Distance(_player.transform.position, transform.position) <= chaseDistance;
        }
    }
}
