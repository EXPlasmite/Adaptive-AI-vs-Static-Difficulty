using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public float fadeInDuration = 1.5f;
    public float fadeOutDuration = 1.5f;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.volume = 0f;
        audioSource.Play();
        StartCoroutine(FadeIn());
        StartCoroutine(TrackLoop());
    }

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

    IEnumerator TrackLoop()
    {
        float waitTime = audioSource.clip.length - fadeOutDuration;
        float timer = 0f;
        while (timer < waitTime)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        float fadeTimer = 0f;
        while (fadeTimer < fadeOutDuration)
        {
            fadeTimer += Time.unscaledDeltaTime;
            audioSource.volume = 1f - (fadeTimer / fadeOutDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.Play();
        StartCoroutine(FadeIn());
        StartCoroutine(TrackLoop());
    }
}