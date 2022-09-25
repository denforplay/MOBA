using Cinemachine;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class CameraInstaller : MonoInstaller
    {
        [SerializeField] private Camera _gameCamera;
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
        
        public override void InstallBindings()
        {
            Container.Bind<Camera>().FromInstance(_gameCamera).AsSingle();
            Container.Bind<CinemachineVirtualCamera>().FromInstance(_cinemachineVirtualCamera).AsSingle();
        }
    }
}