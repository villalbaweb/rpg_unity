using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour 
    {
        // cache
        GameObject _player;
        ActionScheduler _actionScheduler;
        PlayableDirector _playableDirector;

        private void Start() 
        {
            _player = GameObject.FindWithTag("Player");
            _actionScheduler = _player.GetComponent<ActionScheduler>();
            _playableDirector = GetComponent<PlayableDirector>();

            _playableDirector.played += DisableControl;
            _playableDirector.stopped += EnableControl;
        }

        void DisableControl(PlayableDirector director) 
        {
            print("Cancel current Action...");
            _actionScheduler.CancelCurrentAction();
        }
        
        void EnableControl(PlayableDirector director) 
        {
            print("EnableControl...");
        }
    }
}