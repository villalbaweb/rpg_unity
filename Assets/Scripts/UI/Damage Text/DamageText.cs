using UnityEngine;
using UnityEngine.UI;

namespace PRG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] Text textToUpdate = null;

        public void SetDamageText(float damageAmount)
        {
            textToUpdate.text = damageAmount.ToString();
        }
    }
}
