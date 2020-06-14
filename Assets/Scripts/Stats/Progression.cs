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
            return characterClasses.Where(x => x.character == characterClass).FirstOrDefault().health[level - 1];
        }

        [System.Serializable]
        public class ProgressionCharacterClass
        {
            public CharacterClass character;
            public float[] health;
        }
    }
}