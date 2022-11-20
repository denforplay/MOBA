using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.Abstracts;
using Common.Enums;
using Common.EventBus;
using Common.EventBus.Events;
using Controllers;
using Controllers.CombatControllers.Character;
using Cysharp.Threading.Tasks;
using Inputs;
using Models;
using Models.Items;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Views.Abstracts;
using Views.Abstracts.FactoryRequirements;
using Zenject;
using CharacterController = Controllers.CharacterController;
using CharacterInfo = Configurations.Character.CharacterInfo;

namespace Views
{
    public class CharacterView : MonoBehaviour
    {
        public event Action<int> OnSkillActivated;
        [SerializeField] private LevelView _levelView;
        [SerializeField] private ManaView _manaView;
        [SerializeField] private TargetableView _targetableView;
        [SerializeField] private Animator _animator;
        [SerializeField] private CharacterInfo _characterInfo;
        [SerializeField] private NavMeshAgent _navigationAgent;
        [SerializeField] private List<SkillControlBase> _skillControls;

        private ViewFactoryBase<ProjectileView, ProjectileFactoryRequirement> _projectileViewFactory;
        private CharacterController _characterController;
        private CharacterCombatController _characterCombatController;
        private Character _character;
        private Camera _camera;
        private PlayerInputs _playerInputs;
        private ISkillObserver _currentSkillObserver;
        private SkillControlBase _currentSkillControlBase;
        private CancellationTokenSource _cancellationToken;
        private EnemyTargetingInputs _targetingInputs;
        private AnimationController _animationController;

        public CharacterInfo CharacterInfo => _characterInfo;
        public Team Team => _targetableView.Team;
        public Character Character => _character;

        private Dictionary<CombatType, Func<CharacterController, Character, CharacterCombatController>>
            CombatFactory;

        [Inject]
        public void Inject(ViewFactoryBase<ProjectileView, ProjectileFactoryRequirement> projectileViewFactory)
        {
            _projectileViewFactory = projectileViewFactory;
        }

        public void Initialize(Camera camera)
        {
            _camera = camera;
            _character = new Character(_characterInfo, _navigationAgent, _camera)
            {
                Team = Team.Blue
            };
            EventBusManager.GetInstance.Subscribe<OnGameEndedEvent>(DestroyOnGameEnded);
            _levelView.AttachLevelableModel(_character);
            _manaView.AttachManaModel(_character);
            _targetableView.AttachHealthableModel(_character);
            _targetableView.SetTeam(Team.Blue);
            CombatFactory = new Dictionary<CombatType, Func<CharacterController, Character, CharacterCombatController>>()
            {
                { CombatType.Melee, (controller, character) => new MeleeCharacterCombatController(controller, character) },
                {
                    CombatType.Range, (controller, character) =>
                    {
                        var combatController = new RangeCharacterCombatController(controller, character);
                        combatController.Initialize(_projectileViewFactory, new ProjectileFactoryRequirement {ProjectileType = _characterInfo.ProjectileType} );
                        return combatController;
                    }
                }
            };
            
            _skillControls.ForEach(x => x.gameObject.SetActive(false));
            _targetingInputs = new EnemyTargetingInputs(_camera, this);
            _animationController = new AnimationController(_characterInfo.AnimationsInfo, _animator);
            _characterController = new CharacterController(_camera, _navigationAgent, _character, _animationController);
            _characterCombatController = CombatFactory[_characterInfo.CombatType].Invoke(_characterController, _character);
            _playerInputs = new PlayerInputs();
            _playerInputs.Character.Move.canceled += _ =>
            {
                if (!EventSystem.current.IsPointerOverGameObject())//check if click is over ui
                {
                    _targetingInputs.CheckTargetOnClick();
                }
            };
            _targetingInputs.OnTargetedEnemy += _characterCombatController.Attack;
            _playerInputs.Character.Move.canceled += _ =>
            {
                if (!EventSystem.current.IsPointerOverGameObject())//check if click is over ui
                {
                    _characterController.Move();
                }
            };
            _playerInputs.Character.UseFirstSkill.started += _ => StartObserveSkill(0);
            _playerInputs.Character.UseFirstSkill.canceled += _ => StopObserveSkill(0);
            
            _playerInputs.Character.UseSecondSkill.started += _ => StartObserveSkill(1);
            _playerInputs.Character.UseSecondSkill.canceled += _ => StopObserveSkill(1);
            
            _playerInputs.Character.UseThirdSkill.started += _ => StartObserveSkill(2);
            _playerInputs.Character.UseThirdSkill.canceled += _ => StopObserveSkill(2);
            
            _playerInputs.Enable();
        }

        
        public void SetSkillUseState(int skillId, bool canBeUsed)
        {
            _characterController.SetSkillUseState(skillId, canBeUsed);
        }

        private void StartObserveSkill(int skillId)
        {
            var skill = _characterInfo.SkillConfigurations.FirstOrDefault(x => x.Id == skillId);
            _currentSkillControlBase = _skillControls.FirstOrDefault(x => x.SkillType == skill.SkillType);
            if (skill is null || _currentSkillControlBase is null)
                throw new Exception("");

            _cancellationToken?.Dispose();
            _cancellationToken = new CancellationTokenSource();
            _currentSkillObserver = _characterController.ObserveSkill(skill.Id, _cancellationToken);
            if (_currentSkillObserver is null) 
                return;
            
            _currentSkillObserver.OnObservedPositionChanged += _currentSkillControlBase.UpdateSkillView;
            _currentSkillObserver.OnSkillCalled += pos => UseSkill(skillId, pos);
            _currentSkillControlBase.gameObject.SetActive(true);
        }

        private void UseSkill(int skillId, Vector3 onPosition)
        {
            var skillModel = _character.Skills.First(x => x.Id == skillId);
            UniTask.Create(() => skillModel.Apply(onPosition));
        }

        private void Destroy()
        {
            EventBusManager.GetInstance.Unsubscribe<OnGameEndedEvent>(DestroyOnGameEnded);
            _characterCombatController.Cancel();
        }

        private void DestroyOnGameEnded(OnGameEndedEvent e)
        {
            Destroy();
        }
        
        private void StopObserveSkill(int skillId)
        {
            if (_currentSkillObserver is null) 
                return;

            _currentSkillControlBase.gameObject.SetActive(false);
            _cancellationToken.Cancel();
            _currentSkillObserver.OnObservedPositionChanged -= _currentSkillControlBase.UpdateSkillView;
            //_currentSkillObserver.OnSkillCalled -= _character.Skills.FirstOrDefault(x => x.Id == skillId).Apply;
            _currentSkillObserver = null;
            _currentSkillControlBase = null;
            OnSkillActivated?.Invoke(skillId);
        }
        
        public void SubscribeOnMoneyChanged(Action<int> action)
        {
            _character.OnMoneyChanged += action;
        }
        
        public void SubscribeOnInventoryChanged(Action<int, Item> action)
        {
            _character.Inventory.OnItemChanged += action;
        }

        public void EnableInput()
        {
            _playerInputs.Enable();
        }

        public void DisableInput()
        {
            _playerInputs.Disable();
        }
    }
}