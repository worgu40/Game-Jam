using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    public AudioClip cityBackground;
    public AudioClip forestBackground;
    public AudioClip titleScreenBackground;
    public AudioClip jump;
    public AudioClip land;
    public AudioClip swap;
    public AudioClip moving;
    public Slider musicVolumeSlider;
    public Slider SFXVolumeSlider;

    private void Start()
    {
        musicSource.clip = titleScreenBackground;
        musicSource.loop = true;
        musicSource.Play();
    }
    private void Update()
    {
        musicSource.volume = musicVolumeSlider.value;
        SFXSource.volume = SFXVolumeSlider.value;
    }

    public void SwitchToCityMusic()
    {
        musicSource.clip = cityBackground;
        musicSource.Play();
    }

    public void SwitchToForestMusic()
    {
        musicSource.clip = forestBackground;
        musicSource.Play();
    }

    public void PlayJumpSound()
    {
        SFXSource.clip = jump;
        SFXSource.Play();
    }

    public void PlayLandSound()
    {
        SFXSource.clip = land;
        SFXSource.Play();
    }

    public void PlaySwapSound()
    {
        SFXSource.clip = swap;
        SFXSource.Play();
    }

    public void PlayMovingSound()
    {
        if (!SFXSource.isPlaying || SFXSource.clip != moving)
        {
            SFXSource.clip = moving;
            SFXSource.loop = true;
            SFXSource.Play();
        }
    }

    public void StopMovingSound()
    {
        if (SFXSource.clip == moving)
        {
            SFXSource.loop = false;
            SFXSource.Stop();
        }
    }
}
