using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Providers.Assets;
using Services.AudioPlayback;
using Services.ProjectManager;
using StateMachines.DirectControlMultiLayer;
using UnityEngine;
using AudioType = Services.AudioPlayback.AudioType;

namespace Core.Project.Home
{
    public class HomeLoadState : IState, IEnterable
    {
        private readonly IProjectEngine _projectEngine;
        private readonly IAudioPlaybackService _audioPlayback;
        private readonly IAssetsProvider _assetsProvider;

        public HomeLoadState(
            IProjectEngine projectEngine,
            IAudioPlaybackService audioPlayback,
            IAssetsProvider assetsProvider
        )
        {
            _projectEngine = projectEngine;
            _audioPlayback = audioPlayback;
            _assetsProvider = assetsProvider;
        }

        public async UniTask OnEnterAsync(UnitEmpty _)
        {
            await _projectEngine.RunOneShot<LoadHomeSceneState>();
            await _projectEngine.RunOneShot<InstantiatePlayerState>();
            await _projectEngine.RunOneShot<SetupGameEventState>();
            await _projectEngine.RunOneShot<ConfiguredHomeState>();

            await StartSoundtrack();

            await _projectEngine.ChangeState<HomeState>();
        }

        private async UniTask StartSoundtrack()
        {
            var soundtrack = await _assetsProvider.GetAsset<AudioClip>(SoundsPaths.HomeLocationSoundtrack);
            _audioPlayback.PlayAudio(soundtrack, AudioType.Music, loop: true);
        }
    }
}