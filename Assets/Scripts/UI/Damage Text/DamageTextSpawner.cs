using UnityEngine;

namespace PRG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab = null;

        public void Spawn(float damageAmount)
        {
            print("Spawn execute");
            DamageText instance = Instantiate<DamageText>(damageTextPrefab, transform);
        }
    }
}
