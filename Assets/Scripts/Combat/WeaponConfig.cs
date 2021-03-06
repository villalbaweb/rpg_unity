﻿using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Weapon equippedPrefab = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5;
        [SerializeField] float percentageBonus = 0f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string WEAPON_NAME = "Weapon";

        public float WeaponRange { get { return weaponRange; } }
        public float WeaponDamage { get { return weaponDamage; } }
        public float PercentageBonus => percentageBonus;
        public bool HasProjectile { get { return projectile != null; } }

        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(new Transform[] { rightHand, leftHand });

            Weapon weaponInstance = null;

            if(equippedPrefab)
            {
                weaponInstance = Instantiate(equippedPrefab, GetHandTransform(rightHand, leftHand));
                weaponInstance.gameObject.name = WEAPON_NAME;
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

            return weaponInstance;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health _health, GameObject instigator, float statsDamage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(_health, instigator, statsDamage);
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