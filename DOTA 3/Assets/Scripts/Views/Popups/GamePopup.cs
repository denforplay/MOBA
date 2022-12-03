using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Common.Enums;
using Common.PopupSystem;
using Configurations.Items;
using UnityEngine;
using Views.Abstracts;
using Views.Abstracts.FactoryRequirements;
using Zenject;
using CharacterInfo = Configurations.Character.CharacterInfo;

namespace Views.Popups
{
    public class GamePopup : Popup
    {
        [SerializeField] private WayPointsWrapper _wayPointsWrapper;
        [SerializeField] private ShopItemsConfiguration _shopItemsConfiguration;
        [SerializeField] private List<Transform> _leftTeamStartPositions;
        [SerializeField] private List<Transform> _rightTeamStartPositions;
        [SerializeField] private ViewFactoryBase<ProjectileView, ProjectileFactoryRequirement> _projectileViewFactory;
        private Dictionary<int, WayPoint> _wayPointsToPlayerNumber;

        private Camera _camera;
        private HudPopup _hudPopup;
        private PopupSystem _popupSystem;
        private CharacterView _controlledCharacterView;
        private CinemachineVirtualCamera _virtualCamera;

        [Inject]
        public void Inject(PopupSystem popupSystem, CinemachineVirtualCamera virtualCamera, Camera camera)
        {
            _wayPointsToPlayerNumber = new Dictionary<int, WayPoint>()
            {
                { 0, _wayPointsWrapper.TopLineWayPoint },
                { 1, _wayPointsWrapper.MidLineWayPoint },
                { 2, _wayPointsWrapper.BottomLineWayPoint },
            };
            _popupSystem = popupSystem;
            _camera = camera;
            _virtualCamera = virtualCamera;
        }

        public void Initialize(List<CharacterInfo> leftTeamCharacters, List<CharacterInfo> rightTeamCharacters)
        {
            ConfigureActivePlayer(leftTeamCharacters[0]);
            ConfigureTeams(leftTeamCharacters, rightTeamCharacters);
        }

        private void ConfigureActivePlayer(CharacterInfo info)
        {
            var spawnPosition = _leftTeamStartPositions.First();
            var character = Instantiate(info.CharacterPrefab, transform);
            _controlledCharacterView = character.GetComponentInChildren<CharacterView>();
            _controlledCharacterView.Inject(_popupSystem, _projectileViewFactory);
            _hudPopup = _popupSystem.SpawnPopup<HudPopup>(0);
            _controlledCharacterView.InitializePlayer(_camera, spawnPosition, Team.Blue);
            var characterTransform = _controlledCharacterView.transform;
            _virtualCamera.LookAt = characterTransform;
            _virtualCamera.Follow = characterTransform;
            _hudPopup.Initialize(_controlledCharacterView);
            _hudPopup.OnShopCalled += ShowShopPopup;
        }
        
        private void ConfigureTeams(List<CharacterInfo> leftTeamCharacters, List<CharacterInfo> rightTeamCharacters)
        {
            for (int i = 1; i < leftTeamCharacters.Count; i++)
            {
                var spawnPosition = _leftTeamStartPositions[i];
                var character = Instantiate(leftTeamCharacters[i].CharacterPrefab, transform);
                var aiCharacterView = character.GetComponentInChildren<CharacterView>();
                aiCharacterView.Inject(_popupSystem, _projectileViewFactory);
                var startWayPoint = GetStartWayPoint(_wayPointsToPlayerNumber[i], Direction.Right);
                aiCharacterView.InitializeAI(_camera, spawnPosition, startWayPoint, Team.Blue, Direction.Right);
            }
            
            for (int i = 0; i < rightTeamCharacters.Count; i++)
            {
                var spawnPosition = _rightTeamStartPositions[i];
                var character = Instantiate(rightTeamCharacters[i].CharacterPrefab, transform);
                var aiCharacterView = character.GetComponentInChildren<CharacterView>();
                aiCharacterView.Inject(_popupSystem, _projectileViewFactory);
                var startWayPoint = GetStartWayPoint(_wayPointsToPlayerNumber[i], Direction.Left);
                aiCharacterView.InitializeAI(_camera, spawnPosition, startWayPoint, Team.Red, Direction.Left);
            }
            
            _hudPopup.InitializeBattleGroundPanel(leftTeamCharacters, rightTeamCharacters);
        }
        
        private WayPoint GetStartWayPoint(WayPoint wayPoint, Direction direction)
        {
            if (direction == Direction.Left)
            {
                while (wayPoint.NextRightWayPoint is not null)
                {
                    wayPoint = wayPoint.NextRightWayPoint;
                }
            }
            else if (direction == Direction.Right)
            {
                while (wayPoint.NextLeftWayPoint is not null)
                {
                    wayPoint = wayPoint.NextLeftWayPoint;
                }
            }

            return wayPoint;
        }
        
        private void ShowShopPopup()
        {
            var shopPopup = _popupSystem.SpawnPopup<ShopPopup>();
            shopPopup.Initialize(_shopItemsConfiguration, _controlledCharacterView.Character);
            shopPopup.Closing += _ => _controlledCharacterView.EnableInput();
        }
        
        public override void EnableInput()
        {
        }

        public override void DisableInput()
        {
        }
    }
}