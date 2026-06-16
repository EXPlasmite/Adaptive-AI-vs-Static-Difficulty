using UnityEngine;
using System.Collections;

// Handles background music playback with fade in and fade out transitions.
// Loops the track manually to allow smooth fading between repetitions.
public class MusicManager : MonoBehaviour
{
    public float fadeInDuration = 1.5f;
    public float fadeOutDuration = 1.5f;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false; // Manual looping handled by TrackLoop()
        audioSource.volume = 0f;
        audioSource.Play();
        StartCoroutine(FadeIn());
        StartCoroutine(TrackLoop());
    }

    // Gradually increases volume from 0 to 1 over fadeInDuration
    IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer < fadeInDuration)
        {
            timer += Time.unscaledDeltaTime;
            audioSource.volume = timer / fadeInDuration;
            yield return null;
        }
        audioSource.volume = 1f;
    }

    // Waits until near the end of the track, fades out, then restarts
    IEnumerator TrackLoop()
    {
        // Wait until fadeOutDuration seconds before the track ends
        float waitTime = audioSource.clip.length - fadeOutDuration;
        float timer = 0f;
        while (timer < waitTime)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        // Fade out
        float fadeTimer = 0f;
        while (fadeTimer < fadeOutDuration)
        {
            fadeTimer += Time.unscaledDeltaTime;
            audioSource.volume = 1f - (fadeTimer / fadeOutDuration);
            yield return null;
        }

        // Restart track and fade back in
        audioSource.Stop();
        audioSource.Play();
        StartCoroutine(FadeIn());
        StartCoroutine(TrackLoop());
    }
}