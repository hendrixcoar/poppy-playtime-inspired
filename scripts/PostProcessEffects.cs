using UnityEngine;

public class PostProcessEffects : MonoBehaviour
{
    [Header("Vignette Settings")]
    [SerializeField] private bool enableVignette = true;
    [SerializeField] private float vignetteIntensity = 0.3f;
    
    [Header("Chromatic Aberration")]
    [SerializeField] private bool enableChromaticAberration = true;
    [SerializeField] private float aberrationAmount = 0.05f;
    
    [Header("Desaturation")]
    [SerializeField] private bool enableDesaturation = true;
    [SerializeField] private float desaturationAmount = 0.5f;
    
    private Material postProcessMaterial;

    private void Start()
    {
        CreatePostProcessMaterial();
    }

    private void CreatePostProcessMaterial()
    {
        // Create a simple post-process material
        Shader postProcessShader = Shader.Find("Hidden/PostProcess");
        if (postProcessShader != null)
        {
            postProcessMaterial = new Material(postProcessShader);
        }
        else
        {
            Debug.LogWarning("Post-process shader not found");
        }
    }

    public void ApplyVignette(float intensity)
    {
        if (!enableVignette || postProcessMaterial == null) return;
        
        vignetteIntensity = intensity;
    }

    public void ApplyChromatic(float amount)
    {
        if (!enableChromaticAberration || postProcessMaterial == null) return;
        
        aberrationAmount = amount;
    }

    public void ApplyDesaturation(float amount)
    {
        if (!enableDesaturation || postProcessMaterial == null) return;
        
        desaturationAmount = amount;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (postProcessMaterial == null)
        {
            Graphics.Blit(source, destination);
            return;
        }
        
        postProcessMaterial.SetFloat("_VignetteIntensity", vignetteIntensity);
        postProcessMaterial.SetFloat("_ChromaticAmount", aberrationAmount);
        postProcessMaterial.SetFloat("_DesaturationAmount", desaturationAmount);
        
        Graphics.Blit(source, destination, postProcessMaterial);
    }
}
