using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [Header("Health Display")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image healthBar;
    [SerializeField] private Color healthNormalColor = Color.green;
    [SerializeField] private Color healthWarningColor = Color.yellow;
    [SerializeField] private Color healthCriticalColor = Color.red;
    
    [Header("Sanity Display")]
    [SerializeField] private TextMeshProUGUI sanityText;
    [SerializeField] private Image sanityBar;
    [SerializeField] private Color sanityNormalColor = Color.blue;
    [SerializeField] private Color sanityCriticalColor = Color.red;
    
    [Header("Inventory Display")]
    [SerializeField] private TextMeshProUGUI inventoryWeightText;
    [SerializeField] private Image weightBar;
    [SerializeField] private Color weightNormalColor = Color.green;
    [SerializeField] private Color weightWarningColor = Color.red;
    
    [Header("Crosshair")]
    [SerializeField] private Image crosshairImage;
    [SerializeField] private Color crosshairNormalColor = Color.white;
    [SerializeField] private Color crosshairTargetColor = Color.red;
    
    private GameManager gameManager;
    private Inventory inventory;
    private EnemyDetector enemyDetector;

    private void Start()
    {
        gameManager = GameManager.instance;
        inventory = Inventory.instance;
        enemyDetector = FindObjectOfType<EnemyDetector>();
        
        if (inventory != null)
        {
            inventory.OnInventoryChanged += UpdateInventoryDisplay;
        }
    }

    private void Update()
    {
        UpdateHealthDisplay();
        UpdateSanityDisplay();
        UpdateInventoryDisplay();
        UpdateCrosshair();
    }

    private void UpdateHealthDisplay()
    {
        if (gameManager == null) return;
        
        int health = gameManager.GetPlayerHealth();
        int maxHealth = 100;
        
        if (healthText != null)
            healthText.text = $"HP: {health}";
        
        if (healthBar != null)
        {
            float healthPercent = (float)health / maxHealth;
            healthBar.fillAmount = healthPercent;
            
            if (health > 50)
                healthBar.color = healthNormalColor;
            else if (health > 25)
                healthBar.color = healthWarningColor;
            else
                healthBar.color = healthCriticalColor;
        }
    }

    private void UpdateSanityDisplay()
    {
        if (gameManager == null) return;
        
        int sanity = gameManager.GetSanityLevel();
        int maxSanity = 100;
        
        if (sanityText != null)
            sanityText.text = $"Sanity: {sanity}";
        
        if (sanityBar != null)
        {
            float sanityPercent = (float)sanity / maxSanity;
            sanityBar.fillAmount = sanityPercent;
            sanityBar.color = sanity > 30 ? sanityNormalColor : sanityCriticalColor;
        }
    }

    private void UpdateInventoryDisplay()
    {
        if (inventory == null) return;
        
        float weightPercent = inventory.GetWeightPercentage() / 100f;
        
        if (inventoryWeightText != null)
        {
            inventoryWeightText.text = $"Weight: {inventory.GetCurrentWeight():F1}/{inventory.GetMaxWeight():F1}kg";
        }
        
        if (weightBar != null)
        {
            weightBar.fillAmount = weightPercent;
            weightBar.color = weightPercent > 0.8f ? weightWarningColor : weightNormalColor;
        }
    }

    private void UpdateCrosshair()
    {
        if (crosshairImage == null || enemyDetector == null) return;
        
        // Change crosshair color if enemy nearby
        if (enemyDetector.GetNearbyEnemyCount() > 0)
        {
            crosshairImage.color = crosshairTargetColor;
        }
        else
        {
            crosshairImage.color = crosshairNormalColor;
        }
    }
}
