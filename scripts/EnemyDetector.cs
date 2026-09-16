using UnityEngine;
using System.Collections.Generic;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] private float detectionRange = 50f;
    [SerializeField] private LayerMask enemyLayer;
    private List<Enemy> nearbyEnemies = new List<Enemy>();
    
    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        DetectNearbyEnemies();
    }

    private void DetectNearbyEnemies()
    {
        nearbyEnemies.Clear();
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, enemyLayer);
        
        foreach (Collider collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null && enemy.GetCurrentState() != Enemy.EnemyState.Dead)
            {
                nearbyEnemies.Add(enemy);
            }
        }
    }

    public List<Enemy> GetNearbyEnemies() => nearbyEnemies;
    public int GetNearbyEnemyCount() => nearbyEnemies.Count;
    public Enemy GetClosestEnemy()
    {
        if (nearbyEnemies.Count == 0) return null;
        
        Enemy closest = nearbyEnemies[0];
        float closestDistance = Vector3.Distance(transform.position, closest.transform.position);
        
        foreach (Enemy enemy in nearbyEnemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closest = enemy;
                closestDistance = distance;
            }
        }
        
        return closest;
    }
}
