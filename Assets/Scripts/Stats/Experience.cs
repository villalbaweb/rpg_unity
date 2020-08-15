using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;

        // events
        public delegate void ExperienceGainDelegate();
        public event ExperienceGainDelegate OnExperienceGained;

        public void GainExperience(float experience)
        {
            experiencePoints += experience;

            if(OnExperienceGained != null) OnExperienceGained();
        }

        public float GetExperience()
        {
            return experiencePoints;
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}
