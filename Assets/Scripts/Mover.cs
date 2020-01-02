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
        Vector3 targetPos = new Vector3(target.position.x, target.position.y, target.position.z);
        _agent.destination = targetPos;
    }
}
