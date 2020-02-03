using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other) 
        {
            if (other.tag != "Player") return;
            SceneManager.LoadScene(1);
        }
    }
}
