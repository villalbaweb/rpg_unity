using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        [System.Serializable]
        public class ProgressionCharacterClass
        {
            [SerializeField] CharacterClass character;
            [SerializeField] float[] health;
        }
    }
}