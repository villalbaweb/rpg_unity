using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon = null;

        private void OnTriggerEnter(Collider other) 
        {
            if(other.tag != "Player") return;

            Fighter _fighter = other.GetComponent<Fighter>();
            _fighter.EquipWeapon(weapon); 

            Destroy(gameObject);  
        }
    }
}