using RPG.Combat;
using RPG.Core;
using RPG.Movement;
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
        Mover _mover;

        // state
        Vector3 guardLocation;

        private void Start() {
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _player = GameObject.FindWithTag("Player");
            _mover = GetComponent<Mover>();

            guardLocation = transform.position;
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
                _mover.StartMoveAction(guardLocation);
            }
        }

        private bool IsInAttackRange() {
            return Vector3.Distance(_player.transform.position, transform.position) <= chaseDistance;
        }

        // called by Unity
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
