using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 5.0f;

        private void OnTriggerEnter(Collider other) 
        {
            if(other.tag != "Player") return;

            Fighter _fighter = other.GetComponent<Fighter>();
            _fighter.EquipWeapon(weapon); 

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
            foreach(Transform _child in transform)
            {
                _child.gameObject.SetActive(show);
            }

        }
    }
}