using UnityEngine;

public class LightingEffects : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] private Light ambientLight;
    [SerializeField] private Color normalLightColor = Color.white;
    [SerializeField] private Color dangerLightColor = Color.red;
    [SerializeField] private float maxIntensity = 1f;
    [SerializeField] private float minIntensity = 0.5f;
    
    [Header("Flicker Settings")]
    [SerializeField] private bool enableFlicker = false;
    [SerializeField] private float flickerSpeed = 5f;
    [SerializeField] private float flickerAmount = 0.1f;
    
    private float baseIntensity;
    private EnemyDetector enemyDetector;
    private bool isDanger = false;

    private void Start()
    {
        if (ambientLight == null)
        {
            ambientLight = GetComponent<Light>();
        }
        
        baseIntensity = ambientLight.intensity;
        enemyDetector = FindObjectOfType<EnemyDetector>();
    }

    private void Update()
    {
        if (enemyDetector != null)
        {
            bool hasEnemies = enemyDetector.GetNearbyEnemyCount() > 0;
            
            if (hasEnemies != isDanger)
            {
                isDanger = hasEnemies;
                UpdateLightColor();
            }
        }
        
        if (enableFlicker)
        {
            ApplyFlicker();
        }
    }

    private void UpdateLightColor()
    {
        if (ambientLight == null) return;
        
        if (isDanger)
        {
            ambientLight.color = dangerLightColor;
        }
        else
        {
            ambientLight.color = normalLightColor;
        }
    }

    private void ApplyFlicker()
    {
        if (ambientLight == null) return;
        
        float flicker = Mathf.Sin(Time.time * flickerSpeed) * flickerAmount;
        ambientLight.intensity = Mathf.Clamp(baseIntensity + flicker, minIntensity, maxIntensity);
    }
}
