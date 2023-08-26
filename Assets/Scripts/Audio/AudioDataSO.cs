using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicFile {
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
    public MusicFile Key;
    public AudioClip Clip;
}


[CreateAssetMenu(fileName = "AudioData", menuName = "ScriptableObjects/Data/AudioData")]
public class AudioDataSO : ScriptableObject {
    // Sounds
    [SerializeField] private List<AudioFile> musicClipsList;
    [SerializeField] private List<AudioFile> sfxClipsList;

    // Since we cant serialize dicts
    private Dictionary<MusicFile, AudioClip> musicClips = new();
    private Dictionary<MusicFile, AudioClip> sfxClips = new();

    private void OnEnable() {
        foreach (var audioFile in musicClipsList) {
            musicClips[audioFile.Key] = audioFile.Clip;
        }

        foreach (var audioFile in sfxClipsList) {
            sfxClips[audioFile.Key] = audioFile.Clip;
        }
    }

    public AudioClip GetMusicClip(MusicFile key) {
        return musicClips[key];
    }

    public AudioClip GetSFXClip(MusicFile key) {
        return sfxClips[key];
    }

}
