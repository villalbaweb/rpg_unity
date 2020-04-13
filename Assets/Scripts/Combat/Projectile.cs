using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        // config params
        [SerializeField] float speed = 10f;
        
        Health _target = null;

        // Update is called once per frame
        void Update()
        {
            Move();
        }

        public void SetTarget(Health target)
        {
            _target = target;
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
            return _target.transform.position + Vector3.up * targetCapsuleCollider.height / 2;
        }
    }
}
