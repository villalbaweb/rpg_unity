using System;
using System.Collections;
using System.Linq;
using RPG.Control;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        // config params
        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        
        private void OnTriggerEnter(Collider other) 
        {
            if (other.tag != "Player") return;

            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(sceneToLoad); 

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            PlayerController player = FindObjectOfType<PlayerController>();

            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        private Portal GetOtherPortal()
        {
            return FindObjectsOfType<Portal>().Where(x => x != this).FirstOrDefault();
        }
    }
}
