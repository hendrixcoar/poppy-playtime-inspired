using UnityEngine;
using UnityEngine.UI;

public class ScreenFX : MonoBehaviour
{
    public static ScreenFX instance { get; private set; }
    
    [SerializeField] private Image damageFlash;
    [SerializeField] private Image healFlash;
    [SerializeField] private Color damageColor = new Color(1, 0, 0, 0.3f);
    [SerializeField] private Color healColor = new Color(0, 1, 0, 0.3f);
    [SerializeField] private float flashDuration = 0.2f;
    
    [Header("Screen Shake")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float shakeIntensity = 0.1f;
    [SerializeField] private float shakeDuration = 0.1f;
    private Vector3 originalCameraPosition;
    private float shakeTimer = 0f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        
        originalCameraPosition = mainCamera.transform.position;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            ApplyScreenShake();
        }
        else if (mainCamera != null)
        {
            mainCamera.transform.position = originalCameraPosition;
        }
    }

    public void DamageFlash()
    {
        StartCoroutine(FlashScreen(damageFlash, damageColor));
        ScreenShake();
    }

    public void HealFlash()
    {
        StartCoroutine(FlashScreen(healFlash, healColor));
    }

    public void ScreenShake()
    {
        shakeTimer = shakeDuration;
    }

    private void ApplyScreenShake()
    {
        if (mainCamera == null) return;
        
        Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
        mainCamera.transform.position = originalCameraPosition + randomOffset;
    }

    private System.Collections.IEnumerator FlashScreen(Image flashImage, Color flashColor)
    {
        if (flashImage == null) yield break;
        
        flashImage.color = flashColor;
        float elapsed = 0f;
        
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            Color currentColor = flashImage.color;
            currentColor.a = Mathf.Lerp(flashColor.a, 0, elapsed / flashDuration);
            flashImage.color = currentColor;
            yield return null;
        }
        
        flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0);
    }
}
