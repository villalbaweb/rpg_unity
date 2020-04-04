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

        public float WeaponRange { get { return weaponRange; } }
        public float WeaponDamage { get { return weaponDamage; } }

        public void Spawn(Transform handTransform, Animator animator)
        {
            if(equippedPrefab)
            {
                Instantiate(equippedPrefab, handTransform);
            }

            if(animatorOverride)
            {
               animator.runtimeAnimatorController = animatorOverride;
            }
        }
    }
}