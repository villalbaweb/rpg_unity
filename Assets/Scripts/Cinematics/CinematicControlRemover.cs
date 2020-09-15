using RPG.Core;
using RPG.Control;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour 
    {
        // cache
        GameObject _player;
        ActionScheduler _actionScheduler;
        PlayerController _playerController;
        PlayableDirector _playableDirector;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _actionScheduler = _player.GetComponent<ActionScheduler>();
            _playerController = _player.GetComponent<PlayerController>();
            _playableDirector = GetComponent<PlayableDirector>();
        }

        private void OnEnable()
        {
            if(_playableDirector)
            {
                _playableDirector.played += DisableControl;
                _playableDirector.stopped += EnableControl;
            }
        }

        private void OnDisable()
        {
            if(_playableDirector)
            {
                _playableDirector.played -= DisableControl;
                _playableDirector.stopped -= EnableControl;
            }
        }

        void DisableControl(PlayableDirector director) 
        {
            _actionScheduler.CancelCurrentAction();
            _playerController.enabled = false;
        }
        
        void EnableControl(PlayableDirector director) 
        {
            _playerController.enabled = true;
        }
    }
}