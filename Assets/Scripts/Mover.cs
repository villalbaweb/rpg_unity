using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    // config options
    [SerializeField] Transform target;

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
        _agent.destination = target.position;
    }
}
