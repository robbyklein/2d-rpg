using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {
    private AudioSource source;

    private AudioSource Source {
        get {
            if (source == null) {
                source = GetComponent<AudioSource>();
            }
            return source;
        }
    }

    public void PlaySFX(AudioClip clip) {
        Source.PlayOneShot(clip, 0.7f);
    }

    public void ChangeMusic(AudioClip clip) {
        Debug.Log("Change music has been called " + Source);

        StartCoroutine(ChangeMusicRoutine(clip));
    }

    private IEnumerator ChangeMusicRoutine(AudioClip clip) {
        if (Source.clip != null) {
            yield return StartCoroutine(FadeOutMusicRoutine());
        }

        Source.clip = clip;
        Source.volume = 0;
        Source.Play();

        yield return StartCoroutine(FadeInMusicRoutine(1));
    }


    private IEnumerator FadeInMusicRoutine(float targetVolume = 1, float duration = 0.5f) {
        Source.volume = 0;

        while (Source.volume < targetVolume) {
            Source.volume += Time.unscaledDeltaTime / duration;
            yield return null;
        }

        Source.volume = targetVolume;
    }

    public void FadeInMusic(float targetVolume = 1, float duration = 0.5f) {
        StartCoroutine(FadeInMusicRoutine(targetVolume, duration));
    }


    private IEnumerator FadeOutMusicRoutine(float targetVolume = 0, float duration = 0.5f) {
        float startVolume = Source.volume;

        while (Source.volume > targetVolume) {
            Source.volume -= startVolume * Time.unscaledDeltaTime / duration;
            yield return null;
        }

        if (Source.volume <= 0) {
            Source.Stop();
        }
    }

    public void FadeOutMusic(float targetVolume = 0, float duration = 0.5f) {
        StartCoroutine(FadeInMusicRoutine(targetVolume, duration));
    }
}
