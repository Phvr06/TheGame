using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameAudioManager : MonoBehaviour
{
    private static GameAudioManager instance;
    private AudioSource musicSource;
    private AudioSource sfxSource;

    [Header("Audio Clips")]
    [Tooltip("Arraste aqui a musica importada, por exemplo de Assets/Audio/Music.")]
    [SerializeField] private AudioClip backgroundMusic;
    [Tooltip("Som de coleta importado, por exemplo de Assets/Audio/SFX.")]
    [SerializeField] private AudioClip collectClip;
    [Tooltip("Som de dano/importado, por exemplo de Assets/Audio/SFX.")]
    [SerializeField] private AudioClip hitClip;
    [Tooltip("Som de vitoria importado, por exemplo de Assets/Audio/SFX.")]
    [SerializeField] private AudioClip winClip;
    [Tooltip("Som de derrota importado, por exemplo de Assets/Audio/SFX.")]
    [SerializeField] private AudioClip loseClip;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        AudioSource[] sources = GetComponents<AudioSource>();
        musicSource = sources.Length > 0 ? sources[0] : gameObject.AddComponent<AudioSource>();
        sfxSource = sources.Length > 1 ? sources[1] : gameObject.AddComponent<AudioSource>();
        ConfigureSource(musicSource);
        ConfigureSource(sfxSource);
    }

    private void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.volume = 0.18f;
        if (musicSource.clip != null && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public static void PlayCollect()
    {
        if (instance != null && instance.collectClip != null)
        {
            instance.sfxSource.PlayOneShot(instance.collectClip, 0.8f);
        }
    }

    public static void PlayHit()
    {
        if (instance != null && instance.hitClip != null)
        {
            instance.sfxSource.PlayOneShot(instance.hitClip, 0.9f);
        }
    }

    public static void PlayWin()
    {
        if (instance != null && instance.winClip != null)
        {
            instance.sfxSource.PlayOneShot(instance.winClip, 0.95f);
        }
    }

    public static void PlayLose()
    {
        if (instance != null && instance.loseClip != null)
        {
            instance.sfxSource.PlayOneShot(instance.loseClip, 0.95f);
        }
    }

    private static void ConfigureSource(AudioSource source)
    {
        source.playOnAwake = false;
        source.spatialBlend = 0f;
    }
}
