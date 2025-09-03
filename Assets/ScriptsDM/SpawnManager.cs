using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    // ---------- Singleton ----------
    public static SpawnManager Instance { get; private set; } // global accessor

    private void Awake()
    {
        // ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
           
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    [Header("Prefabs & Pool")]
    [SerializeField] private List<GameObject> _aiPrefabs; 
    [SerializeField] private int _poolSize = 30;          // fixed pool size

    [Header("Waypoints (Scene Transforms)")]
    [SerializeField] private Transform _startWaypoint;    // where AI starts
    [SerializeField] private Transform _endWaypoint;      // where AI ends

    [Header("Spawning")]
    [SerializeField] private float _spawnInterval = 1.0f; // seconds between spawns
    [SerializeField] private int _maxActiveAtOnce = 30;   // safety cap

    private readonly List<GameObject> _pool = new List<GameObject>();
    private float _nextSpawnTime;

    private void Start()
    {
        if (_aiPrefabs == null || _aiPrefabs.Count == 0)
        {
            Debug.LogError("SpawnManager: No AI prefabs assigned.");
            return;
        }
        if (_startWaypoint == null || _endWaypoint == null)
        {
            Debug.LogError("SpawnManager: Assign StartWaypoint and EndWaypoint in the Inspector.");
            return;
        }

        // --- Build pool (pre-instantiate) ---
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject prefab = _aiPrefabs[Random.Range(0, _aiPrefabs.Count)];
            GameObject aiGO = Instantiate(prefab);
            aiGO.SetActive(false); // not in use yet

            if (aiGO.GetComponent<AI>() == null)
            {
                aiGO.AddComponent<AI>(); 
            }

            _pool.Add(aiGO);
        }
    }

    private void Update()
    {
        if (Time.time >= _nextSpawnTime)
        {
            _nextSpawnTime = Time.time + _spawnInterval;

            int activeCount = 0;
            foreach (var ai in _pool)
                if (ai.activeInHierarchy) activeCount++;

            if (activeCount >= _maxActiveAtOnce) return;

            SpawnOne();
        }
    }

    public void SpawnOne()
    {
        foreach (var aiGO in _pool)
        {
            if (!aiGO.activeInHierarchy)
            {
                var ai = aiGO.GetComponent<AI>();
                ai.Initialize(_startWaypoint, _endWaypoint);

                aiGO.SetActive(true); // OnEnable handles movement
                return;
            }
        }
    }
}
