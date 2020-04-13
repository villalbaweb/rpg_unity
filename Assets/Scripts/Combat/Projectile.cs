using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        // config params
        [SerializeField] float speed = 10f;
        
        Health _target = null;
        float _damage = 0;

        // Update is called once per frame
        void Update()
        {
            Move();
        }

        public void SetTarget(Health target, float damage)
        {
            _target = target;
            _damage = damage;
        }

        private void Move()
        {
            if (!_target) return;

            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsuleCollider = _target.GetComponent<CapsuleCollider>();
            
            if(!targetCapsuleCollider) 
            {
                return _target.transform.position;
            }

            return _target.transform.position + Vector3.up * targetCapsuleCollider.height / 2;
        }

        private void OnTriggerEnter(Collider other) 
        {
            if(other.GetComponent<Health>() != _target) return;

            _target.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
