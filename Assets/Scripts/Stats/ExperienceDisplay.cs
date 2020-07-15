using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        // cache
        Experience _playerExperience;
        Text _text;

        private void Awake() 
        {
            _playerExperience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            _text = GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            float experience = _playerExperience.GetExperience();
            _text.text = experience > 0 
                ? string.Format("{0}", experience)
                : "0";
        }
    }
}
