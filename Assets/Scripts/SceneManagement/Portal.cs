using System;
using System.Collections;
using System.Linq;
using RPG.Control;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A,
            B
        }

        // config params
        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 0.5f;
        [SerializeField] float fadeInTime = 0.5f;
        
        private void OnTriggerEnter(Collider other) 
        {
            if (other.tag != "Player") return;

            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);

            Fader _fader = FindObjectOfType<Fader>();
            yield return _fader.FadeOut(fadeOutTime);

            SavingWrapper _savingWrapper = FindObjectOfType<SavingWrapper>();
            _savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad); 

            _savingWrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            yield return _fader.FadeIn(fadeInTime);

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            NavMeshAgent _agent = player.GetComponent<NavMeshAgent>();
            _agent.enabled = false;
            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            _agent.enabled = true;
        }

        private Portal GetOtherPortal()
        {
            Portal identifiedPortal = FindObjectsOfType<Portal>().Where(x => x != this && x.destination == destination).FirstOrDefault();
            return identifiedPortal;
        }
    }
}
