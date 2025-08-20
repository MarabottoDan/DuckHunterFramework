using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [SerializeField] private List<Transform> _waypoints;
    private NavMeshAgent _agent;
    private int _currentPoint = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        if (_agent != null)
        {
            _agent.destination = _waypoints[0].position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_agent.remainingDistance < 0.5f && !_agent.pathPending)
        {
            // Check if we are at the last waypoint
            if (_currentPoint >= _waypoints.Count - 1)
            {
                Destroy(gameObject); // destroy AI
                return; // exit Update to avoid out-of-range
            }

            // Move to next waypoint
            _currentPoint++;
            _agent.SetDestination(_waypoints[_currentPoint].position);
        }

    }
    
}
