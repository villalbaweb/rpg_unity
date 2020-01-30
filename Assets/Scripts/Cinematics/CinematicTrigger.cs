using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        // state
        bool IsCinematicTriggered = false;

        private void OnTriggerEnter(Collider other) {

            if(IsCinematicTriggered) return;

            Debug.Log("colliding...");

            GetComponent<PlayableDirector>().Play();
            IsCinematicTriggered = true;
        }
    }
}

