using System.Collections;
using RPG.Control;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthPickup : MonoBehaviour, IRayCastable
    {
        [SerializeField] float healthToHeal = 20;
        [SerializeField] float respawnTime = 5.0f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;

            Health _health = other.GetComponent<Health>();
            Pickup(_health);
        }

        private void Pickup(Health health)
        {
            health.Heal(healthToHeal);

            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);

            yield return new WaitForSeconds(seconds);

            ShowPickup(true);
        }

        private void ShowPickup(bool show)
        {
            GetComponent<CapsuleCollider>().enabled = show;
            foreach (Transform _child in transform)
            {
                _child.gameObject.SetActive(show);
            }

        }

        public bool HandleRaycast(PlayerController playerController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(playerController.GetComponent<Health>());
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Heal;
        }
    }
}
