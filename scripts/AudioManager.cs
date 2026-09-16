using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [System.Serializable]
    public class AudioClipData
    {
        public string clipID;
        public AudioClip clip;
        public float volume = 1f;
        public bool loop = false;
    }

    [SerializeField] private List<AudioClipData> audioClips = new List<AudioClipData>();
    [SerializeField] private float masterVolume = 1f;
    
    private Dictionary<string, AudioSource> activeSources = new Dictionary<string, AudioSource>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(string clipID, Vector3 position = default)
    {
        AudioClipData data = audioClips.Find(a => a.clipID == clipID);
        if (data == null)
        {
            Debug.LogWarning($"Audio clip not found: {clipID}");
            return;
        }

        AudioSource source = new GameObject($"Audio_{clipID}").AddComponent<AudioSource>();
        source.clip = data.clip;
        source.volume = data.volume * masterVolume;
        source.loop = data.loop;
        source.transform.position = position;
        source.PlayOneShot(data.clip);
        
        Destroy(source.gameObject, data.clip.length);
    }

    public void PlayAmbientMusic(string clipID)
    {
        if (activeSources.ContainsKey("music"))
        {
            Destroy(activeSources["music"].gameObject);
            activeSources.Remove("music");
        }

        AudioClipData data = audioClips.Find(a => a.clipID == clipID);
        if (data == null)
        {
            Debug.LogWarning($"Audio clip not found: {clipID}");
            return;
        }

        AudioSource source = new GameObject("Music_Source").AddComponent<AudioSource>();
        source.clip = data.clip;
        source.volume = data.volume * masterVolume;
        source.loop = true;
        source.Play();
        activeSources["music"] = source;
    }

    public void StopMusic()
    {
        if (activeSources.ContainsKey("music"))
        {
            Destroy(activeSources["music"].gameObject);
            activeSources.Remove("music");
        }
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
    }
}
