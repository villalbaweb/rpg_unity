using System.Linq;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        public float GetHealth(CharacterClass characterClass, int level)
        {
            float health = characterClasses
                .Where(x => x.character == characterClass)
                .FirstOrDefault()
                .stats
                .Where(stat => stat.stat == Stat.Health)
                .FirstOrDefault()
                .levels[level - 1];

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