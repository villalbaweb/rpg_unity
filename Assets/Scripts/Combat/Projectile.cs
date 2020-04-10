using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        // config params
        [SerializeField] float speed = 10f;
        [SerializeField] Transform target = null;

        // Update is called once per frame
        void Update()
        {
            Move();
        }

        private void Move()
        {
            if (!target) return;

            transform.LookAt(target);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
