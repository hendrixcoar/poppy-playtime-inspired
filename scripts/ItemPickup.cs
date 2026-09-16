using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private int quantity = 1;
    [SerializeField] private float pickupRange = 2f;
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private Sprite pickupIcon;
    
    private bool hasBeenPickedUp = false;
    private Transform playerTransform;

    private void Start()
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerTransform = playerController.transform;
        }
    }

    private void Update()
    {
        // Rotate item for visual appeal
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        
        // Check for player proximity
        if (playerTransform != null && !hasBeenPickedUp)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            
            if (distanceToPlayer <= pickupRange)
            {
                AttemptPickup();
            }
        }
    }

    public void AttemptPickup()
    {
        if (item == null || hasBeenPickedUp)
            return;
        
        if (Inventory.instance.AddItem(item, quantity))
        {
            hasBeenPickedUp = true;
            Debug.Log($"Picked up: {item.itemName} x{quantity}");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventory is full!");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            AttemptPickup();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
