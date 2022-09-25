using Common.PopupSystem;
using Models.Factories;
using UnityEngine;
using Views.Popups;
using Zenject;

namespace Installers
{
    public class PopupsInstaller : MonoInstaller
    {
        [SerializeField] PopupSystem _popupSystem;

        public override void InstallBindings()
        {
            Container.BindFactory<Object, Transform, GamePopup, PopupFactory<GamePopup>>().AsSingle();
            Container.BindFactory<Object, Transform, MainMenuPopup, PopupFactory<MainMenuPopup>>().AsSingle();
            Container.Bind<PopupSystem>().FromInstance(_popupSystem).AsSingle();
            _popupSystem.Initialize(Container);
        }
    }
}