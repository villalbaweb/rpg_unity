using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        // cache
        BaseStats _playerBaseStats;
        Text _text;

        private void Awake()
        {
            _playerBaseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            _text = GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            float experience = _playerBaseStats.CurrentLevel.value;
            _text.text = experience > 0
                ? string.Format("{0}", experience)
                : "0";
        }
    }
}