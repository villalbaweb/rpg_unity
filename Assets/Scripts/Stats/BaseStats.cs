﻿using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        public float GetStat(Stat statToGet)
        {
            return progression ? progression.GetStat(statToGet, characterClass, GetExperienceLevel()) : 0;
        }

        public int GetExperienceLevel()
        {
            Experience _experience = GetComponent<Experience>();

            if(!_experience) { return startingLevel; }

            float currentXP = _experience.GetExperience();
            int penultimateLevel = progression ? progression.GetLevels(Stat.ExperienceToLevelUp, characterClass) : 0;
            for(int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUp = progression ? progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level) : 0;
                if (currentXP < XPToLevelUp)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }
    }
}
