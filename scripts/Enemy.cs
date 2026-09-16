using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private string enemyName = "Enemy";
    [SerializeField] private int health = 50;
    [SerializeField] private float damagePerHit = 10f;
    [SerializeField] private float attackCooldown = 2f;
    private float lastAttackTime = 0f;
    
    [Header("Detection")]
    [SerializeField] private float detectionRange = 30f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float fieldOfView = 120f;
    
    [Header("Movement")]
    [SerializeField] private float patrolSpeed = 3.5f;
    [SerializeField] private float chaseSpeed = 6f;
    [SerializeField] private float stoppingDistance = 0.5f;
    
    [Header("Patrol Points")]
    [SerializeField] private Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    
    private NavMeshAgent navMeshAgent;
    private Transform playerTransform;
    private EnemyState currentState = EnemyState.Patrol;
    private float stateChangeTimer = 0f;
    
    public enum EnemyState
    {
        Patrol,
        Alert,
        Chase,
        Attack,
        Dead
    }

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerTransform = FindObjectOfType<PlayerController>()?.transform;
        
        if (navMeshAgent == null)
        {
            Debug.LogError($"NavMeshAgent component not found on {gameObject.name}");
        }
        
        if (patrolPoints.Length == 0)
        {
            Debug.LogWarning($"No patrol points assigned to {gameObject.name}");
        }
    }

    private void Update()
    {
        if (currentState == EnemyState.Dead) return;
        
        // Check player detection
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            bool playerDetected = IsPlayerDetected(distanceToPlayer);
            
            if (playerDetected)
            {
                if (distanceToPlayer <= attackRange)
                {
                    ChangeState(EnemyState.Attack);
                }
                else
                {
                    ChangeState(EnemyState.Chase);
                }
            }
            else if (currentState == EnemyState.Chase)
            {
                ChangeState(EnemyState.Alert);
            }
        }
        
        // Handle current state
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Alert:
                Alert();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }
    }

    private bool IsPlayerDetected(float distance)
    {
        if (distance > detectionRange)
            return false;
        
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        
        if (angleToPlayer > fieldOfView / 2f)
            return false;
        
        // Line of sight check
        if (Physics.Raycast(transform.position, directionToPlayer, distance))
        {
            return true;
        }
        
        return true;
    }

    private void Patrol()
    {
        navMeshAgent.speed = patrolSpeed;
        
        if (patrolPoints.Length == 0) return;
        
        Transform targetPoint = patrolPoints[currentPatrolIndex];
        navMeshAgent.SetDestination(targetPoint.position);
        
        if (!navMeshAgent.hasPath) return;
        
        if (navMeshAgent.remainingDistance <= stoppingDistance)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    private void Alert()
    {
        navMeshAgent.speed = patrolSpeed;
        stateChangeTimer += Time.deltaTime;
        
        if (stateChangeTimer > 3f)
        {
            ChangeState(EnemyState.Patrol);
        }
    }

    private void Chase()
    {
        if (playerTransform == null) return;
        
        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.SetDestination(playerTransform.position);
    }

    private void Attack()
    {
        navMeshAgent.velocity = Vector3.zero;
        
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            DealDamageToPlayer();
            lastAttackTime = Time.time;
        }
        
        // Look at player
        if (playerTransform != null)
        {
            Vector3 directionToPlayer = playerTransform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(directionToPlayer);
        }
    }

    private void DealDamageToPlayer()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.DamagePlayer((int)damagePerHit);
            Debug.Log($"{enemyName} attacked player for {damagePerHit} damage!");
        }
    }

    private void ChangeState(EnemyState newState)
    {
        if (currentState == newState) return;
        
        currentState = newState;
        stateChangeTimer = 0f;
        Debug.Log($"{enemyName} changed state to: {newState}");
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"{enemyName} took {damage} damage. Health: {health}");
        
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        ChangeState(EnemyState.Dead);
        navMeshAgent.enabled = false;
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 2f);
    }

    public EnemyState GetCurrentState() => currentState;
    public int GetHealth() => health;
}
