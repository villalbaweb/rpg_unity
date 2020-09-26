using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        // config
        [SerializeField] Health healthComponent = null;
        [SerializeField] Canvas rootCanvas = null;

        void Start() 
        {
            EnableCanvas(false);
        }

        void OnEnable() 
        {
            if(!healthComponent) return;

            healthComponent.OnTakeDamage += OnHealthDamage;    
        }

        void OnDisable() 
        {
            if(!healthComponent) return;

            healthComponent.OnTakeDamage -= OnHealthDamage;    
        }

        void OnHealthDamage()
        {
            if (!healthComponent) return;

            float lifePropertyLeft = healthComponent.HealthPoints / healthComponent.MaxHealthPoints;

            transform.localScale = new Vector3(lifePropertyLeft, 1, 1);

            EnableCanvas(lifePropertyLeft > Mathf.Epsilon);
        }

        private void EnableCanvas(bool enableSignal)
        {
            if(!rootCanvas) return;

            rootCanvas.enabled = enableSignal;
        }
    }
}
