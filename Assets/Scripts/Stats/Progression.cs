using System.Linq;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            ProgessionStat progressionStat = characterClasses
                .Where(x => x.character == characterClass)
                .FirstOrDefault()
                .stats
                .Where(x =>x.levels.Length >= level && x.stat == stat)
                .FirstOrDefault();

                float health = progressionStat != null ? progressionStat.levels[level - 1] : 0;

            return health;
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