using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnZone
    {
        public string zoneName;
        public Transform spawnPoint;
        public int maxEnemies = 3;
        public float spawnRadius = 5f;
        public GameObject enemyPrefab;
    }

    [SerializeField] private List<SpawnZone> spawnZones = new List<SpawnZone>();
    [SerializeField] private float spawnCheckInterval = 5f;
    private float spawnCheckTimer = 0f;
    
    private Dictionary<string, List<Enemy>> spawnedEnemies = new Dictionary<string, List<Enemy>>();

    private void Start()
    {
        InitializeSpawnZones();
    }

    private void InitializeSpawnZones()
    {
        foreach (SpawnZone zone in spawnZones)
        {
            if (!spawnedEnemies.ContainsKey(zone.zoneName))
            {
                spawnedEnemies[zone.zoneName] = new List<Enemy>();
            }
        }
    }

    private void Update()
    {
        spawnCheckTimer += Time.deltaTime;
        
        if (spawnCheckTimer >= spawnCheckInterval)
        {
            CheckAndSpawnEnemies();
            spawnCheckTimer = 0f;
        }
    }

    private void CheckAndSpawnEnemies()
    {
        foreach (SpawnZone zone in spawnZones)
        {
            if (zone.enemyPrefab == null) continue;
            
            List<Enemy> zoneEnemies = spawnedEnemies[zone.zoneName];
            
            // Remove dead enemies from list
            zoneEnemies.RemoveAll(e => e == null);
            
            // Spawn new enemies if below max
            while (zoneEnemies.Count < zone.maxEnemies)
            {
                SpawnEnemyInZone(zone);
            }
        }
    }

    private void SpawnEnemyInZone(SpawnZone zone)
    {
        Vector3 randomOffset = Random.insideUnitSphere * zone.spawnRadius;
        randomOffset.y = 0; // Keep on ground
        Vector3 spawnPosition = zone.spawnPoint.position + randomOffset;
        
        GameObject enemyObject = Instantiate(zone.enemyPrefab, spawnPosition, Quaternion.identity);
        Enemy enemy = enemyObject.GetComponent<Enemy>();
        
        if (enemy != null)
        {
            spawnedEnemies[zone.zoneName].Add(enemy);
            Debug.Log($"Spawned enemy in zone: {zone.zoneName}");
        }
    }

    public int GetEnemyCountInZone(string zoneName)
    {
        if (spawnedEnemies.ContainsKey(zoneName))
        {
            spawnedEnemies[zoneName].RemoveAll(e => e == null);
            return spawnedEnemies[zoneName].Count;
        }
        return 0;
    }

    public int GetTotalEnemyCount()
    {
        int total = 0;
        foreach (var zone in spawnedEnemies)
        {
            zone.Value.RemoveAll(e => e == null);
            total += zone.Value.Count;
        }
        return total;
    }
}
