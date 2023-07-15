using UnityEngine;
using UnityEngine.AI;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public EnemyRobot robot;
    public WaveController wavecontroller;
    private NavMeshAgent agent;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        //agent.updatePosition = false;
        agent.updateRotation = false;
        
        agent.SetDestination(transform.position + transform.forward);
    }

    private void Update()
    {
        // - Debug
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawLine(ray.origin, ray.direction, Color.blue, 1.0f);
            if (Physics.Raycast(ray, out var hit))
                agent.SetDestination(hit.point);
        }
        // -

        robot.AddMovementInput(agent.remainingDistance > agent.stoppingDistance ? agent.desiredVelocity : Vector3.zero);
        //robot.AddMovementInput(agent.desiredVelocity);
    }

    public void StopMovement()
    {
        agent.isStopped = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Post reached: " + other.gameObject.name);

        var postScript = other.GetComponent<Post>();
        if (postScript == null) 
            return;

        postScript.PostWasReached();
        if (postScript.GetNextPost() == null)
            return;

        agent.SetDestination(postScript.GetNextPost().position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Arrow>() == null) 
            return;
        wavecontroller.EnemyKilled();
        Destroy(gameObject);
    }
}
