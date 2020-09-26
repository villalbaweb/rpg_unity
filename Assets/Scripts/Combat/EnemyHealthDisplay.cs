using System;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        // cache
        Fighter _playerFighter;
        Health _health;
        Text _text;

        private void Awake()
        {
            _playerFighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            _text = GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            _health = _playerFighter.GetTarget();
            _text.text = _health && _health.GetPercentage() > 0 
                ? String.Format("{0:0.0} / {1:0.0}", _health.HealthPoints, _health.MaxHealthPoints) 
                : "N/A";
        }
    }
}
