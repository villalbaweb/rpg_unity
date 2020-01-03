using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    // cache
    NavMeshAgent _agent;
    Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            MoveToCursor();
        }
        UpdateAnimator();
    }

    private void MoveToCursor() 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);
        RaycastHit hitData;
        bool hasHit = Physics.Raycast(ray, out hitData);

        _agent.destination = hasHit ? hitData.point : transform.position;
    }

    private void UpdateAnimator() 
    {
        Vector3 localVelocity = transform.InverseTransformDirection(_agent.velocity);
        float speed = localVelocity.z;
        _animator.SetFloat("ForwardSpeed", speed);
    }
}
