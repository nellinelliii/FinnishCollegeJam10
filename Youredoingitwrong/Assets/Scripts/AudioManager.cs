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

    void Start()
    {
        if (ambienceSource != null)
        {
            ambienceSource.loop = true;
            ambienceSource.Play();
        }
    }

    public void PlayChime()
    {
        int level = ValidationSystem.Instance.currentLevel;

        if (level <= 3)
        {
            chimeSource.pitch = 1f;
            chimeSource.volume = 0.8f;
        }
        else if (level <= 6)
        {
            chimeSource.pitch = Mathf.Lerp(1f, 0.88f, (level - 3) / 3f);
            chimeSource.volume = 0.7f;
        }
        else
        {
            chimeSource.pitch = Random.Range(0.7f, 0.85f);
            chimeSource.volume = 0.5f;
        }

        chimeSource.PlayOneShot(chimeClip);
    }

    void Update()
    {
        if (ambienceSource == null) return;
        int level = ValidationSystem.Instance?.currentLevel ?? 1;
        float targetVolume = Mathf.Lerp(0.4f, 0.05f, (level - 1) / 9f);
        ambienceSource.volume = Mathf.MoveTowards(
            ambienceSource.volume, targetVolume, Time.deltaTime * 0.1f);
    }
}