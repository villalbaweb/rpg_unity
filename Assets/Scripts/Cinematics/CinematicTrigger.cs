using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other) 
        {
            if(other.tag != "Player") return;

            GetComponent<BoxCollider>().enabled = false;
            GetComponent<PlayableDirector>().Play();
        }
    }
}

