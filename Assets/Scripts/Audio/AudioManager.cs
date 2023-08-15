using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    // Data
    [SerializeField] private AudioDataSO data;

    // Players
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    // Settings
    [SerializeField] private MusicFile initialSong;


    public void Start() {
        DontDestroyOnLoad(this.gameObject);
        ChangeMusic(initialSong);
    }

    public void ChangeMusic(MusicFile song) {
        StartCoroutine(ChangeMusicRoutine(song));
    }

    private IEnumerator ChangeMusicRoutine(MusicFile song) {
        if (musicSource.clip != null) {
            yield return StartCoroutine(FadeOutMusic());
        }

        musicSource.clip = data.GetMusicClip(song);
        musicSource.volume = 0;
        musicSource.Play();

        yield return StartCoroutine(FadeInMusic());
    }

    private IEnumerator FadeInMusic(float duration = 1f) {
        musicSource.volume = 0;

        while (musicSource.volume < 1.0f) {
            musicSource.volume += Time.deltaTime / duration;
            yield return null;
        }

        musicSource.volume = 1.0f;
    }

    private IEnumerator FadeOutMusic(float duration = 1.0f) {
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0) {
            musicSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        musicSource.Stop();
    }
}
