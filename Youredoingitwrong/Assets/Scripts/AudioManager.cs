using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("The One Sound")]
    public AudioSource chimeSource;
    public AudioClip chimeClip;

    [Header("Ambience")]
    public AudioSource ambienceSource;

    void Awake() => Instance = this;

    /// <summary>
    /// Call this on any puzzle event. The same sound plays,
    /// but its character changes with the level.
    /// </summary>
    public void PlayChime()
    {
        int level = ValidationSystem.Instance.currentLevel;

        // Early: bright, normal pitch
        if (level <= 3)
        {
            chimeSource.pitch = 1f;
            chimeSource.volume = 0.8f;
        }
        // Mid: slightly flat
        else if (level <= 6)
        {
            chimeSource.pitch = Mathf.Lerp(1f, 0.88f, (level - 3) / 3f);
            chimeSource.volume = 0.7f;
        }
        // Late: noticeably off, quiet
        else
        {
            chimeSource.pitch = Random.Range(0.7f, 0.85f);
            chimeSource.volume = 0.5f;
        }

        chimeSource.PlayOneShot(chimeClip);
    }

    /// <summary>
    /// Call when Observer speaks — small "hm" sound.
    /// In late game: silence is more disturbing, so skip it.
    /// </summary>
    public void PlayObserverHum()
    {
        int level = ValidationSystem.Instance.currentLevel;
        if (level >= 9) return; // silence
        // Play whatever small sound you have assigned to chimeSource
        // or add a second AudioSource for the Observer
    }

    /// <summary>
    /// Fade ambience down as game progresses.
    /// </summary>
    void Update()
    {
        if (ambienceSource == null) return;
        int level = ValidationSystem.Instance?.currentLevel ?? 1;
        float targetVolume = Mathf.Lerp(0.4f, 0.05f, (level - 1) / 9f);
        ambienceSource.volume = Mathf.MoveTowards(ambienceSource.volume, targetVolume, Time.deltaTime * 0.1f);
    }
}