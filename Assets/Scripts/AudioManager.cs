using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public AudioClip GameMusic;
    public AudioClip gameOverMusic;
    public AudioClip mainMenuMusic;
    public static AudioManager instance;

    private AudioSource audioSource; // Background music
    private AudioSource collectibleAudioSource; // For collectible sounds

    public List<AudioClip> collectibleSounds; // Sequential sounds
    private int collectibleSoundIndex = 0;     // Tracks the current sound index
    private Coroutine resetCoroutine;          // Coroutine to handle reset logic
    public float musicVolume = 1f; // Range from 0 to 1 for music volume
    public float soundVolume = 1f; // Range from 0 to 1 for sound effects volume

    // Method to set music volume
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        audioSource.volume = musicVolume; // Adjust music volume
    }

    // Method to set sound effects volume
    public void SetSoundVolume(float volume)
    {
        soundVolume = volume;
        collectibleAudioSource.volume = soundVolume; // Adjust sound effects volume
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Destroy duplicate
            return;              // Exit to avoid setting audioSource for a destroyed object
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on AudioManager GameObject.");
        }

        // Add a second AudioSource for collectible sounds
        collectibleAudioSource = gameObject.AddComponent<AudioSource>();
        if (collectibleAudioSource == null)
        {
            Debug.LogError("Failed to create collectibleAudioSource.");
        }
    }

    void Start()
    {
        // Play the music based on the initial scene
        PlayMusicForScene(SceneManager.GetActiveScene().name);

    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    void PlayMusicForScene(string sceneName)
    {
        FadeOutAndPlayNewMusic(GetMusicClipForScene(sceneName), 1f);
    }

    AudioClip GetMusicClipForScene(string sceneName)
    {
        switch (sceneName)
        {
            case "Level1":
                return GameMusic;
            case "GameOverScene":
                return gameOverMusic;
            case "MainMenu":
                return mainMenuMusic;
            default:
                return null;
        }
    }

    public void PlayNextCollectibleSound()
    {
        if (collectibleSounds.Count == 0) return;

        // Apply sound volume
        collectibleAudioSource.volume = soundVolume;

        // Play the current collectible sound
        collectibleAudioSource.clip = collectibleSounds[collectibleSoundIndex];
        collectibleAudioSource.Play();

        collectibleSoundIndex = Mathf.Min(collectibleSoundIndex + 1, collectibleSounds.Count - 1);

        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }
        resetCoroutine = StartCoroutine(ResetSoundSequenceAfterDelay());
    }


    private IEnumerator ResetSoundSequenceAfterDelay()
    {
        // Wait for 2 seconds without a new collectible
        yield return new WaitForSeconds(2f);

        // Reset the sound index to the start
        collectibleSoundIndex = 0;
    }

    public void PlayRepeatingLastSound()
    {
        if (collectibleSounds.Count == 0 || collectibleSoundIndex != collectibleSounds.Count - 1) return;

        // Repeat the last sound indefinitely
        collectibleAudioSource.loop = true;
        collectibleAudioSource.clip = collectibleSounds[collectibleSounds.Count - 1];
        collectibleAudioSource.Play();
    }

    public void StopRepeatingSound()
    {
        collectibleAudioSource.loop = false;
    }

    public void FadeOutAndPlayNewMusic(AudioClip newClip, float fadeDuration = 1f)
    {
        if (newClip == null) return;

        StartCoroutine(FadeOutCoroutine(newClip, fadeDuration));
    }

    private IEnumerator FadeOutCoroutine(AudioClip newClip, float fadeDuration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;

        audioSource.clip = newClip;
        audioSource.Play();

        while (audioSource.volume < 1)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
}
