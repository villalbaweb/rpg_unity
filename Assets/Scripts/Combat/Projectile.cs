using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        // config params
        [SerializeField] float speed = 10f;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifetime = 5.0f;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float timeAfterImpact = 0.2f;
        
        Health _target = null;
        GameObject _instigator = null;
        float _damage = 0;

        private void Start() 
        {
            transform.LookAt(GetAimLocation());
        }
        // Update is called once per frame
        void Update()
        {
            Move();
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            _target = target;
            _damage = damage;
            _instigator = instigator;

            Destroy(gameObject, maxLifetime);
        }

        private void Move()
        {
            if (!_target) return;

            if(isHoming && !_target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
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
            if(other.GetComponent<Health>() != _target || _target.IsDead()) return;

            DestroyProjectilePartsOnHit();

            HitParticleFx();

            _target.TakeDamage(_instigator, _damage);
            
            Destroy(gameObject, timeAfterImpact);
        }

        private void DestroyProjectilePartsOnHit()
        {
            foreach(GameObject projectilePart in destroyOnHit)
            {
                Destroy(projectilePart);
            }
        }

        private void HitParticleFx()
        {
            if(!hitEffect) return;
            
            Instantiate(hitEffect, GetAimLocation(), Quaternion.identity);
        }
    }
}
