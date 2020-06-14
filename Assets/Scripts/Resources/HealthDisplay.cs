using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resources
{
    public class HealthDisplay : MonoBehaviour
    {
        // cache
        Health _health;
        Text _text;

        private void Awake()
        {
            _health = GameObject.FindWithTag("Player").GetComponent<Health>();
            _text = GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            _text.text = String.Format("{0:0.0} %", _health.GetPercentage());
        }
    }
}
