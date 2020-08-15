using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;

        // events
        public event Action OnExperienceGainedEvent;

        public void GainExperience(float experience)
        {
            experiencePoints += experience;

            if(OnExperienceGainedEvent != null) OnExperienceGainedEvent();
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
