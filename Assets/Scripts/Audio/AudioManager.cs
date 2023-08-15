using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    // Data
    [SerializeField] private AudioDataSO data;
    [SerializeField] private GameManagerSO gameManager;

    // Players
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    // Settings
    [SerializeField] private MusicFile initialSong;

    // State
    private MusicFile currentMusic;

    #region Lifecycle
    private void OnEnable() {
        gameManager.onGameStateChange += HandleGameStateChange;
    }

    private void OnDisable() {
        gameManager.onGameStateChange -= HandleGameStateChange;
    }

    private void Start() {
        DontDestroyOnLoad(this.gameObject);
        ChangeMusic(initialSong);
    }
    #endregion

    public void ChangeMusic(MusicFile song) {
        if (currentMusic != song) {
            currentMusic = song;
            StartCoroutine(ChangeMusicRoutine(song));
        }
        else {
            StartCoroutine(FadeInMusic(1));

        }
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

    private IEnumerator FadeInMusic(float targetVolume = 1, float duration = 0.5f) {
        musicSource.volume = 0;

        while (musicSource.volume < targetVolume) {
            musicSource.volume += Time.unscaledDeltaTime / duration;
            yield return null;
        }

        musicSource.volume = targetVolume;
    }

    private IEnumerator FadeOutMusic(float targetVolume = 0, float duration = 0.5f) {
        float startVolume = musicSource.volume;

        while (musicSource.volume > targetVolume) {
            musicSource.volume -= startVolume * Time.unscaledDeltaTime / duration;
            yield return null;
        }

        if (musicSource.volume <= 0) {
            musicSource.Stop();
        }
    }

    private void HandleGameStateChange(GameState state) {
        // TODO: clean this up
        switch (state) {
            case GameState.World:
                ChangeMusic(MusicFile.World);
                break;
            case GameState.Pause:
                StartCoroutine(FadeOutMusic(0.1f));
                break;
            case GameState.Inventory:
                StartCoroutine(FadeOutMusic(0.1f));
                break;
            case GameState.Battle:
                ChangeMusic(MusicFile.Battle);
                break;
        }
    }

}
