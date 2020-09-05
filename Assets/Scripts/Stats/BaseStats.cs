using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpParticleEffect = null;

        // cache
        Experience _experience;

        // state
        public int CurrentLevel { get; set; }

        void Start()
        {
            _experience = GetComponent<Experience>();

            if(_experience)
            {
                _experience.OnExperienceGainedEvent += OnExperienceGained;
            }

            CurrentLevel = GetExperienceLevel();
        }

        void OnDestroy()
        {
            if(_experience)
            {
                _experience.OnExperienceGainedEvent -= OnExperienceGained;
            }
        }

        public float GetStat(Stat statToGet)
        {
            return progression ? progression.GetStat(statToGet, characterClass, CurrentLevel) : 0;
        }

        private int GetExperienceLevel()
        {
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

        private void OnExperienceGained()
        {
            int calculatedLevel = GetExperienceLevel();

            if(calculatedLevel > CurrentLevel)
            {
                CurrentLevel = calculatedLevel;
                LevelUpVFX();
            }
        }

        private void LevelUpVFX()
        {
            var levelUpParticles = Instantiate(levelUpParticleEffect, transform);
            Destroy(levelUpParticles, 2);
        }
    }
}
