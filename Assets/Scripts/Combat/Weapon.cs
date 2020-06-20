using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string WEAPON_NAME = "Weapon";

        public float WeaponRange { get { return weaponRange; } }
        public float WeaponDamage { get { return weaponDamage; } }
        public bool HasProjectile { get { return projectile != null; } }

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(new Transform[] { rightHand, leftHand });

            if(equippedPrefab)
            {
                GameObject weaponInstance = Instantiate(equippedPrefab, GetHandTransform(rightHand, leftHand));
                weaponInstance.name = WEAPON_NAME;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (animatorOverride)
            {
               animator.runtimeAnimatorController = animatorOverride;
            }
            else if(overrideController)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health _health, GameObject instigator)
        {
            Projectile projectileInstance = Instantiate(projectile, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(_health, instigator, weaponDamage);
        }

        private void DestroyOldWeapon(Transform[] hands)
        {
            foreach(Transform hand in hands)
            {
                Transform oldWeapon = hand.Find(WEAPON_NAME);
                if(oldWeapon) 
                {
                    Destroy(oldWeapon.gameObject);
                }
            }

        }

        private Transform GetHandTransform(Transform rightHand, Transform leftHand)
        {
            return isRightHanded ? rightHand : leftHand;
        }
    }
}