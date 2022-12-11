using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.Abstracts;
using Common.Enums;
using Controllers.Interfaces;
using Cysharp.Threading.Tasks;
using Models;
using Models.Skills.Observers;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Views;
using Views.SkillControls;

namespace Controllers
{
    public class CharacterController : ICharacterController
    {
        private Camera _camera;
        private NavMeshAgent _navigation;
        private PlayerInputs _inputs;
        private readonly Character _controlledCharacter;
        private Vector3 _skillPosition;
        private Dictionary<SkillType, ISkillObserver> _skillObservers;
        private readonly AnimationController _animationController;
        private bool _isMoving;
        private readonly CharacterView _characterView;

        public CharacterController(Camera camera, CharacterView characterView, Character 
            controlledCharacter, AnimationController animationController)
        {
            _camera = camera;
            _characterView = characterView;
            _navigation = characterView.NavigationAgent;
            _animationController = animationController;
            _inputs = new PlayerInputs();
            _controlledCharacter = controlledCharacter;
            _skillObservers = new Dictionary<SkillType, ISkillObserver>();
            _skillObservers.Add(SkillType.Dash, new DirectedSkillObserver(_camera));
            _skillObservers.Add(SkillType.RangeDamage, new RangeTargetZoneSkillObserver(_camera));
            _skillObservers.Add(SkillType.Self, new SelfSkillObserver());
        }

        public bool IsDestroyed => _characterView.IsDestroyed;
        public NavMeshAgent NavigationAgent => _navigation;

        public void Move()
        {
            var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
            {
                var distance = Vector3.Distance(_navigation.transform.position, hit.point);
                if (distance > _navigation.stoppingDistance)
                {
                    _navigation.speed = _controlledCharacter.Speed;
                    _navigation.SetDestination(hit.point);
                    _animationController.ChangeAnimation(AnimationType.Walk);
                    if (!_isMoving)
                        UniTask.Create(ObserveMovingAnimation);
                }
            }
        }

        public void MoveTo(Vector3 point)
        {
            _navigation.ResetPath();
            point.y = _navigation.transform.position.y;
            _navigation.destination = point;
        }


        private async UniTask ObserveMovingAnimation()
        {
            _isMoving = true;
            while (Math.Abs(_navigation.remainingDistance - _navigation.stoppingDistance) > 0.1f)
            {
                await UniTask.Delay(TimeSpan.FromMilliseconds(10));
            }

            _animationController.ChangeAnimation(AnimationType.Idle);
            _isMoving = false;
        }
        
        public void SetState(AnimationType animationType)
        {
            _animationController.ChangeAnimation(animationType);
        }


        public void SetSkillUseState(int skillId, bool canBeUsed)
        {
            var skill = _controlledCharacter.Skills.FirstOrDefault(x => x.Id == skillId);
            skill.CanBeUsed = canBeUsed;
        }

        public ISkillObserver ObserveSkill(int skillId, CancellationTokenSource cancellationToken)
        {
            var skill = _controlledCharacter.Skills.FirstOrDefault(x => x.Id == skillId);
            if (skill is not null && skill.CanBeUsed)
            {
                var skillObserver = _skillObservers[skill.SkillType];
                skillObserver.ObserveSkill(cancellationToken);
                return skillObserver;
            }
            else
            {
                return null;
            }
        }
    }
}