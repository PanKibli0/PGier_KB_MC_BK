using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    public AudioClip menuMusic;
    public AudioClip battleMusic;
    public AudioClip campfire;

    [Header("UI")]
    public AudioClip buttonClick;
    public AudioClip characterSelect;
    public AudioClip levelSelect;

    [Header("Combat")]
    public AudioClip attackWoj;
    public AudioClip attackMag;
    public AudioClip attackDruid;
    public AudioClip shield;
    public AudioClip healing;
    public AudioClip cardPlay;
    public AudioClip victory;
    public AudioClip death;
    public AudioClip summon;

    [Header("Enemies")]
    public AudioClip deathOther;
    public AudioClip attackOther;

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
        }
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null)
            return;

        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
            return;

        sfxSource.PlayOneShot(clip);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp01(volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp01(volume);
    }
}