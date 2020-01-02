using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    // config options
    [SerializeField] Transform target;

    // cache
    NavMeshAgent _agent;
    Ray lastRay;


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
            lastRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        Debug.DrawRay(lastRay.origin, lastRay.direction * 100, Color.green);
        _agent.destination = target.position;
    }
}
