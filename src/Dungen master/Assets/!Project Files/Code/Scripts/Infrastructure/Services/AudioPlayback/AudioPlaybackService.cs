using System.Collections.Generic;
using UnityEngine;

namespace Services.AudioPlayback
{
    public class AudioPlaybackService : IAudioPlaybackService
    {
        private readonly Dictionary<AudioType, AudioSource> _audioSources = new();

        public AudioPlaybackService(
            AudioSource musicSource,
            AudioSource soundEffectSource,
            AudioSource voiceOverSource
        )
        {
            _audioSources[AudioType.Music] = musicSource;
            _audioSources[AudioType.SoundEffect] = soundEffectSource;
            _audioSources[AudioType.VoiceOver] = voiceOverSource;
        }

        public void PlayAudio(string audioName, AudioType audioType, bool loop = false)
        {
            if (!_audioSources.TryGetValue(audioType, out var audioSource))
            {
                Debug.LogError($"Аудио источник для типа {audioType} не найден.");
                return;
            }

            audioSource.clip = Resources.Load<AudioClip>(audioName);
            audioSource.loop = loop;
            audioSource.Play();
        }
        
        public void PlayAudio(AudioClip audioClip, AudioType audioType, bool loop = false)
        {
            if (!_audioSources.TryGetValue(audioType, out var audioSource))
            {
                Debug.LogError($"Аудио источник для типа {audioType} не найден.");
                return;
            }

            audioSource.clip = audioClip;
            audioSource.loop = loop;
            audioSource.Play();
        }

        public void StopAudio(AudioType audioType)
        {
            if (!_audioSources.TryGetValue(audioType, out var audioSource))
            {
                Debug.LogError($"Аудио источник для типа {audioType} не найден.");
                return;
            }

            audioSource.Stop();
        }

        public void PauseAudio(AudioType audioType)
        {
            if (!_audioSources.TryGetValue(audioType, out var audioSource))
            {
                Debug.LogError($"Аудио источник для типа {audioType} не найден.");
                return;
            }

            audioSource.Pause();
        }

        public void ResumeAudio(AudioType audioType)
        {
            if (!_audioSources.TryGetValue(audioType, out var audioSource))
            {
                Debug.LogError($"Аудио источник для типа {audioType} не найден.");
                return;
            }

            audioSource.UnPause();
        }

        public void SetVolume(float volume, AudioType audioType)
        {
            if (!_audioSources.TryGetValue(audioType, out var audioSource))
            {
                Debug.LogError($"Аудио источник для типа {audioType} не найден.");
                return;
            }

            audioSource.volume = volume;
        }

        public float GetVolume(AudioType audioType)
        {
            if (!_audioSources.TryGetValue(audioType, out var audioSource))
            {
                Debug.LogError($"Аудио источник для типа {audioType} не найден.");
                return 0f;
            }

            return audioSource.volume;
        }
    }
}