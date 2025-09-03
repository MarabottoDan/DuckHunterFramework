using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AI : MonoBehaviour
{
    [SerializeField] private float _arriveThreshold = 0.5f; // distance treated as arrived to EndWayPoint

    private NavMeshAgent _agent;
    private Transform _start;
    private Transform _end;

    // Called by SpawnManager before enabling
    public void Initialize(Transform start, Transform end)
    {
        _start = start;
        _end = end;
    }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        if (_agent == null) _agent = GetComponent<NavMeshAgent>();
        if (_start == null || _end == null)
        {
            gameObject.SetActive(false);
            return;
        }

        // Warp to start and set destination
        _agent.Warp(_start.position);
        _agent.ResetPath();
        _agent.SetDestination(_end.position);
    }

    private void Update()
    {
        if (_agent.pathPending) return;

        if (_agent.remainingDistance <= _arriveThreshold)
        {
            // Arrived: return to pool
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        // reset agent so it's fresh when reused
        if (_agent != null)
        {
            _agent.velocity = Vector3.zero;
            _agent.ResetPath();
        }
    }
}
