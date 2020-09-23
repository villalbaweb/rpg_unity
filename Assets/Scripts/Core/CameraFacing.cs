using UnityEngine;

namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {
        void Update()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
