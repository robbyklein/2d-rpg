using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundClip {
    World,
    Menu,
    Battle,
    Boss,
    GameOver,
    Attack,
    MenuChange,
    MenuSelect,
}

[System.Serializable]
struct AudioFile {
    public SoundClip Key;
    public AudioClip Clip;
}


[CreateAssetMenu(fileName = "AudioManager", menuName = "ScriptableObjects/Managers/AudioManager")]
public class AudioManagerSO : ScriptableObject {
    [SerializeField] private GameManagerSO gameManager;
    [SerializeField] private GameObject audioPlayerPrefab;

    private AudioPlayer musicPlayer;
    private AudioPlayer sfxPlayer;

    // Sound clips
    [SerializeField] private List<AudioFile> soundClipsList;
    private Dictionary<SoundClip, AudioClip> soundClips = new();

    private void OnEnable() {
        // Subscribe to scene change
        gameManager.onGameStateChange += HandleGameStateChange;

        // Build sound clip dictionary
        foreach (var audioFile in soundClipsList) {
            soundClips[audioFile.Key] = audioFile.Clip;
        }
    }

    private void OnDisable() {
        gameManager.onGameStateChange -= HandleGameStateChange;
    }

    private AudioPlayer MusicPlayer {
        get {
            if (musicPlayer == null) {
                musicPlayer = Instantiate(audioPlayerPrefab).GetComponent<AudioPlayer>();
                DontDestroyOnLoad(musicPlayer.gameObject);
            }
            return musicPlayer;
        }
    }

    private AudioPlayer SFXPlayer {
        get {
            if (sfxPlayer == null) {
                sfxPlayer = Instantiate(audioPlayerPrefab).GetComponent<AudioPlayer>();
                DontDestroyOnLoad(sfxPlayer.gameObject);
            }
            return sfxPlayer;
        }
    }

    public void ChangeMusic(SoundClip key) {
        MusicPlayer.ChangeMusic(GetSoundClip(key));
    }

    public void PlaySFX(SoundClip key) {
        SFXPlayer.PlaySFX(GetSoundClip(key));
    }

    private AudioClip GetSoundClip(SoundClip key) {
        return soundClips[key];
    }

    private void HandleGameStateChange(GameState state) {
        switch (state) {
            case GameState.World:
                ChangeMusic(SoundClip.World);
                break;
            case GameState.Pause:
                MusicPlayer.FadeOutMusic(0.1f);
                break;
            case GameState.Inventory:
                MusicPlayer.FadeOutMusic(0.1f);
                break;
            case GameState.Battle:
                ChangeMusic(SoundClip.Battle);
                break;
            case GameState.GameOver:
                ChangeMusic(SoundClip.GameOver);
                break;
            case GameState.MainMenu:
                ChangeMusic(SoundClip.Menu);
                break;
        }
    }
}
