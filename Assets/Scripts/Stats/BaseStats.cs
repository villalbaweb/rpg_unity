using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        public float GetHealth()
        {
            return progression ? progression.GetHealth(characterClass, startingLevel) : 0;
        }

        public float GetExperienceReward()
        {
            return 10;
        }
    }
}
