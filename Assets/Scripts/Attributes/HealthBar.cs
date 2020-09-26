using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        // cache
        Health _health;
        Canvas _canvas;

        void Awake() 
        {
            _health = GetComponentInParent<Health>();
            _canvas = GetComponentInParent<Canvas>();
        }

        void Start() 
        {
            EnableCanvas(false);
        }

        void OnEnable() 
        {
            if(!_health) return;

            _health.OnTakeDamage += OnHealthDamage;    
        }

        void OnDisable() 
        {
            if(!_health) return;

            _health.OnTakeDamage -= OnHealthDamage;    
        }

        void OnHealthDamage()
        {
            if (!_health) return;

            float lifePropertyLeft = _health.HealthPoints / _health.MaxHealthPoints;

            transform.localScale = new Vector3(lifePropertyLeft, 1, 1);

            EnableCanvas(lifePropertyLeft > Mathf.Epsilon);
        }

        private void EnableCanvas(bool enableSignal)
        {
            if(!_canvas) return;

            _canvas.enabled = enableSignal;
        }
    }
}
