using System;
using Common.Abstracts;
using Common.PopupSystem;
using UnityEngine;
using Views.Popups;
using Views.Towers;
using Zenject;

namespace CompositeRoots
{
    public class TowerCompositeRoot : CompositeRoot
    {
        [SerializeField] private TowerView _redTeamMainTower;
        [SerializeField] private TowerView _blueTeamMainTower;
        private PopupSystem _popupSystem;

        [Inject]
        public void Inject(PopupSystem popupSystem)
        {
            _popupSystem = popupSystem;
        }

        private void Start()
        {
            Compose();
        }

        public override void Compose()
        {
            _blueTeamMainTower.SubscribeOnHealthEnded(WinBlueTeam);
            _redTeamMainTower.SubscribeOnHealthEnded(WinReadTeam);
        }

        private void WinBlueTeam()
        {
            _popupSystem.SpawnPopup<GameResultsPopup>();
        }
        
        private void WinReadTeam()
        {
            _popupSystem.SpawnPopup<GameResultsPopup>();
        }
    }
}