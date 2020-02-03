using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        // config params
        [SerializeField] int sceneToLoad = -1;
        
        private void OnTriggerEnter(Collider other) 
        {
            if (other.tag != "Player") return;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
