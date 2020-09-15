using System;
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
        [SerializeField] bool shouldUseModifiers = false;

        // events
        public event Action OnLevelUpEvent;

        // cache
        Experience _experience;

        // state
        public int CurrentLevel { get; set; }

        private void Awake()
        {
            _experience = GetComponent<Experience>();
        }

        void Start()
        {
            CurrentLevel = GetExperienceLevel();
        }

        private void OnEnable()
        {
            if(_experience)
            {
                _experience.OnExperienceGainedEvent += OnExperienceGained;
            }
        }

        private void OnDisable()
        {
            if(_experience)
            {
                _experience.OnExperienceGainedEvent -= OnExperienceGained;
            }
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
            return progression
                ? (GetBaseStat(statToGet) + GetAdditiveModifier(statToGet)) * (1 + GetPercentageModifier(statToGet)/100)
                : 0;
        }

        private float GetBaseStat(Stat statToGet)
        {
            return progression.GetStat(statToGet, characterClass, CurrentLevel);
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if(!shouldUseModifiers) return 0;

            float additiveModifier = 0;
            foreach(IModifierProvider modifierProvider in GetComponents<IModifierProvider>())
            {
                foreach(float modifierValue in modifierProvider.GetAdditiveModifiers(stat))
                {
                    additiveModifier += modifierValue;
                }
            }

            return additiveModifier;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if(!shouldUseModifiers) return 0;
            
            float percentageModifier = 0;
            foreach(IModifierProvider modifierProvider in GetComponents<IModifierProvider>())
            {
                foreach(float modifierValue in modifierProvider.GetPercentageModifiers(stat))
                {
                    percentageModifier += modifierValue;
                }
            }

            return percentageModifier;
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
                if(OnLevelUpEvent != null) OnLevelUpEvent();
            }
        }

        private void LevelUpVFX()
        {
            var levelUpParticles = Instantiate(levelUpParticleEffect, transform);
            Destroy(levelUpParticles, 2);
        }
    }
}
