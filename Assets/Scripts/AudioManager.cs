using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Start() => audioSource = GetComponent<AudioSource>();

    public static void PlayGunSFX(AudioClip clip) => Instance.audioSource.PlayOneShot(clip);
}
