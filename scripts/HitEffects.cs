using UnityEngine;

public class HitEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem bloodSprayPrefab;
    [SerializeField] private ParticleSystem impactSparksPrefab;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private float bloodParticleCount = 20f;
    [SerializeField] private float impactParticleCount = 15f;
    
    private Enemy enemy;
    private Health health;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        health = GetComponent<Health>();
    }

    public void PlayHitEffect(Vector3 hitPoint, Vector3 hitNormal)
    {
        // Play blood spray
        if (bloodSprayPrefab != null)
        {
            ParticleSystem bloodEffect = Instantiate(bloodSprayPrefab, hitPoint, Quaternion.LookRotation(hitNormal));
            var emission = bloodEffect.emission;
            emission.rateOverTime = bloodParticleCount;
            Destroy(bloodEffect.gameObject, 2f);
        }
        
        // Play impact sparks
        if (impactSparksPrefab != null)
        {
            ParticleSystem impactEffect = Instantiate(impactSparksPrefab, hitPoint, Quaternion.LookRotation(hitNormal));
            var emission = impactEffect.emission;
            emission.rateOverTime = impactParticleCount;
            Destroy(impactEffect.gameObject, 1f);
        }
        
        // Play hit sound
        if (hitSound != null && AudioManager.instance != null)
        {
            AudioManager.instance.PlaySound(hitSound.name);
        }
        
        // Screen shake
        if (ScreenFX.instance != null)
        {
            ScreenFX.instance.ScreenShake();
        }
    }
}
