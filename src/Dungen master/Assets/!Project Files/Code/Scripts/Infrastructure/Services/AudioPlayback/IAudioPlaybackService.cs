using UnityEngine;

namespace Services.AudioPlayback
{
    public interface IAudioPlaybackService
    {
        void PlayAudio(string audioName, AudioType audioType, bool loop = false);
        void PlayAudio(AudioClip audioClip, AudioType audioType, bool loop = false);
        void StopAudio(AudioType audioType);
        
        void PauseAudio(AudioType audioType);
        void ResumeAudio(AudioType audioType);
        
        void SetVolume(float volume, AudioType audioType);
        float GetVolume(AudioType audioType);
    }
}