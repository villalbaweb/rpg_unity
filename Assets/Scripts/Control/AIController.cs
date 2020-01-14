using RPG.Combat;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        // config params
        [SerializeField] float chaseDistance = 5f;

        // cache
        Fighter _fighter;
        GameObject _player;

        private void Start() {
            _fighter = GetComponent<Fighter>();
            _player = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
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
