using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [SerializeField] private List<Transform> _waypoints;
    private NavMeshAgent _agent;
    
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
        
    }
}
