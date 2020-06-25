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
            return progression ? progression.GetStat(Stat.Health, characterClass, startingLevel) : 0;
        }

        public float GetExperienceReward()
        {
            return progression ? progression.GetStat(Stat.ExperienceReward, characterClass, startingLevel) : 0;
        }
    }
}
