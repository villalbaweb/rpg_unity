using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();

            float statValue = 0;

            if(lookupTable.ContainsKey(characterClass) && lookupTable[characterClass].ContainsKey(stat))
            {
                float[] statLevels = lookupTable[characterClass][stat];

                if (statLevels.Length >= level)
                {
                    statValue = statLevels[level - 1];
                }
            }

            return statValue;
        }

        private void BuildLookup()
        {
            if(lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach(ProgressionCharacterClass progressionClass in characterClasses)
            {
                Dictionary<Stat, float[]> statsLookupTable = new Dictionary<Stat, float[]>();
                foreach(ProgessionStat progressionStat in progressionClass.stats)
                {
                    statsLookupTable.Add(progressionStat.stat, progressionStat.levels);
                }

                lookupTable.Add(progressionClass.character, statsLookupTable);
            }
        }

        [System.Serializable]
        public class ProgressionCharacterClass
        {
            public CharacterClass character;
            public ProgessionStat[] stats;
        }

        [System.Serializable]
        public class ProgessionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}