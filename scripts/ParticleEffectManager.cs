using UnityEngine;

public class ParticleEffectManager : MonoBehaviour
{
    public static ParticleEffectManager instance { get; private set; }
    
    [System.Serializable]
    public class ParticleEffect
    {
        public string effectName;
        public ParticleSystem particleSystemPrefab;
    }
    
    [SerializeField] private ParticleEffect[] particleEffects;
    private System.Collections.Generic.Dictionary<string, ParticleSystem> effectCache;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        InitializeEffectCache();
    }

    private void InitializeEffectCache()
    {
        effectCache = new System.Collections.Generic.Dictionary<string, ParticleSystem>();
        
        foreach (ParticleEffect effect in particleEffects)
        {
            if (effect.particleSystemPrefab != null)
            {
                effectCache[effect.effectName] = effect.particleSystemPrefab;
            }
        }
        
        Debug.Log($"Particle effect cache initialized with {effectCache.Count} effects");
    }

    public void PlayEffect(string effectName, Vector3 position, Quaternion rotation)
    {
        if (effectCache.ContainsKey(effectName))
        {
            ParticleSystem original = effectCache[effectName];
            ParticleSystem instance = Instantiate(original, position, rotation);
            
            float duration = instance.main.duration + instance.main.startLifetime.constantMax;
            Destroy(instance.gameObject, duration);
            
            Debug.Log($"Playing effect: {effectName}");
        }
        else
        {
            Debug.LogWarning($"Particle effect not found: {effectName}");
        }
    }

    public void PlayEffect(string effectName, Transform target)
    {
        PlayEffect(effectName, target.position, target.rotation);
    }
}
