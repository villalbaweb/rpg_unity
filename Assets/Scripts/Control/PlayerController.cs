using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour {
        
        // cache
        Mover _mover;
        
        private void Start() {
            _mover = GetComponent<Mover>();
        }

        private void Update() {
            if (Input.GetMouseButton(0))
            {
                MoveToCursor();
            }
        }

        private void MoveToCursor()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);    // just debug
            RaycastHit hitData;
            bool hasHit = Physics.Raycast(ray, out hitData);
            Vector3 newPosition = hasHit ? hitData.point : transform.position;
            _mover.MoveTo(newPosition);
        }
    }
}