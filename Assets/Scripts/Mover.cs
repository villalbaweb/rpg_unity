using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    // cache
    NavMeshAgent _agent;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            MoveToCursor();
        }
    }

    private void MoveToCursor() 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);
        RaycastHit hitData;
        bool hasHit = Physics.Raycast(ray, out hitData);

        _agent.destination = hasHit ? hitData.point : transform.position;
    }
}
