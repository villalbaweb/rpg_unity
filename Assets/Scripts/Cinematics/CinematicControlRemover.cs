using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour 
    {
        // cache
        PlayableDirector _playableDirector;

        private void Start() 
        {
            _playableDirector = GetComponent<PlayableDirector>();

            _playableDirector.played += DisableControl;
            _playableDirector.stopped += EnableControl;
        }

        void DisableControl(PlayableDirector director) 
        {
            print("DisableControl...");
        }
        
        void EnableControl(PlayableDirector director) 
        {
            print("EnableControl...");
        }
    }
}