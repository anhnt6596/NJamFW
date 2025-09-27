using UnityEngine;
using Core;

public class SoundManager : MonoBehaviour
{
    private static SoundManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private bool musicEnabled;
    private bool sfxEnabled;

    private void Awake()
    {
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

        musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        sfxEnabled = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;

        ApplySettings();
    }

    // ================== MUSIC ==================
    public static void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (!Instance.musicEnabled || clip == null) return;

        Instance.musicSource.clip = clip;
        Instance.musicSource.loop = loop;
        Instance.musicSource.Play();
    }
    public static void StopMusic() => Instance.musicSource.Stop();

    // ================== SFX ==================
    public static void Play(AudioClip clip) => Instance.PlaySFX(clip);
    private void PlaySFX(AudioClip clip)
    {
        if (!sfxEnabled || clip == null) return;

        sfxSource.PlayOneShot(clip);
    }

    // ================== TOGGLE ==================
    public static void ToggleMusic()
    {
        Instance.musicEnabled = !Instance.musicEnabled;
        PlayerPrefs.SetInt("MusicEnabled", Instance.musicEnabled ? 1 : 0);
        Instance.ApplySettings();
    }

    public static void ToggleSFX()
    {
        Instance.sfxEnabled = !Instance.sfxEnabled;
        PlayerPrefs.SetInt("SFXEnabled", Instance.sfxEnabled ? 1 : 0);
        Instance.ApplySettings();
    }

    private void ApplySettings()
    {
        musicSource.mute = !musicEnabled;
        sfxSource.mute = !sfxEnabled;
    }

    // ================== GET STATE ==================
    public static bool IsMusicEnabled() => Instance.musicEnabled;
    public static bool IsSFXEnabled() => Instance.sfxEnabled;
}