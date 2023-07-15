using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIRobotController : MonoBehaviour
{
    public Robot robot;
    
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        agent.updatePosition = false;
        agent.updateRotation = false;
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

        // Move Robot with the desired velocity
        robot.AddMovementInput(agent.remainingDistance > agent.stoppingDistance ? agent.desiredVelocity : Vector3.zero);
    }
}
