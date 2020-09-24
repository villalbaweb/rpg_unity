using UnityEngine;

namespace PRG.UI.DamageText
{
    public class Destroyer : MonoBehaviour
    {
        [SerializeField] GameObject targetToDestroy = null;

        public void DestroyTarget()
        {
            if(targetToDestroy)
            {
                Destroy(targetToDestroy);
            }
        }
    }
}
