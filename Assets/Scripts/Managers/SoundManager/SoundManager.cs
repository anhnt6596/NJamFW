using UnityEngine;
using Core;

public class SoundManager : MonoBehaviour, IManager
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private bool musicEnabled;
    private bool sfxEnabled;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Load state t? PlayerPrefs
        musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        sfxEnabled = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;

        ApplySettings();
    }

    // ================== MUSIC ==================
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (!musicEnabled || clip == null) return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    // ================== SFX ==================
    public void PlaySFX(AudioClip clip)
    {
        if (!sfxEnabled || clip == null) return;

        sfxSource.PlayOneShot(clip);
    }

    // ================== TOGGLE ==================
    public void ToggleMusic()
    {
        musicEnabled = !musicEnabled;
        PlayerPrefs.SetInt("MusicEnabled", musicEnabled ? 1 : 0);
        ApplySettings();
    }

    public void ToggleSFX()
    {
        sfxEnabled = !sfxEnabled;
        PlayerPrefs.SetInt("SFXEnabled", sfxEnabled ? 1 : 0);
        ApplySettings();
    }

    private void ApplySettings()
    {
        musicSource.mute = !musicEnabled;
        sfxSource.mute = !sfxEnabled;
    }

    // ================== GET STATE ==================
    public bool IsMusicEnabled() => musicEnabled;
    public bool IsSFXEnabled() => sfxEnabled;

    public void Init()
    {

    }

    public void StartUp()
    {

    }

    public void Cleanup()
    {

    }
}