using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    
    [System.Serializable]
    public class AudioClipReference
    {
        public string soundName;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }
    
    [SerializeField] private AudioClipReference[] soundLibrary;
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private float masterVolume = 1f;
    
    private System.Collections.Generic.Dictionary<string, AudioClipReference> soundCache;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        InitializeSoundCache();
    }

    private void InitializeSoundCache()
    {
        soundCache = new System.Collections.Generic.Dictionary<string, AudioClipReference>();
        
        foreach (AudioClipReference audio in soundLibrary)
        {
            if (audio.clip != null)
            {
                soundCache[audio.soundName] = audio;
            }
        }
        
        Debug.Log($"Audio cache initialized with {soundCache.Count} sounds");
    }

    public void PlaySound(string soundName, Vector3 position = default)
    {
        if (soundCache.ContainsKey(soundName))
        {
            AudioClipReference audioRef = soundCache[soundName];
            
            if (position != default)
            {
                // 3D audio at position
                AudioSource.PlayClipAtPoint(audioRef.clip, position, audioRef.volume * masterVolume);
            }
            else if (sfxAudioSource != null)
            {
                // 2D audio through SFX source
                sfxAudioSource.PlayOneShot(audioRef.clip, audioRef.volume * masterVolume);
            }
            
            Debug.Log($"Playing sound: {soundName}");
        }
        else
        {
            Debug.LogWarning($"Sound not found: {soundName}");
        }
    }

    public void PlayMusic(string musicName, bool loop = true)
    {
        if (soundCache.ContainsKey(musicName) && musicAudioSource != null)
        {
            AudioClipReference audioRef = soundCache[musicName];
            musicAudioSource.clip = audioRef.clip;
            musicAudioSource.volume = audioRef.volume * masterVolume;
            musicAudioSource.loop = loop;
            musicAudioSource.Play();
            
            Debug.Log($"Playing music: {musicName}");
        }
    }

    public void StopMusic()
    {
        if (musicAudioSource != null)
        {
            musicAudioSource.Stop();
        }
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        if (musicAudioSource != null) musicAudioSource.volume = masterVolume;
        if (sfxAudioSource != null) sfxAudioSource.volume = masterVolume;
    }
}
