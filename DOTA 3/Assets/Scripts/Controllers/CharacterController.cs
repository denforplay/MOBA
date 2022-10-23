using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.Abstracts;
using Common.Enums;
using Controllers.AnimationStateControllers;
using Cysharp.Threading.Tasks;
using Models;
using Models.Skills.Observers;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class CharacterController : IController
    {
        private Camera _camera;
        private NavMeshAgent _navigation;
        private PlayerInputs _inputs;
        private readonly Character _controlledCharacter;
        private Vector3 _skillPosition;
        private Dictionary<SkillType, ISkillObserver> _skillObservers;
        private readonly AnimationController _animationController;
        private bool _isMoving;
        public AnimationController AnimationController => _animationController;

        public CharacterController(Camera camera, NavMeshAgent navAgent, Character 
            controlledCharacter, AnimationController animationController)
        {
            _camera = camera;
            _navigation = navAgent;
            _animationController = animationController;
            _inputs = new PlayerInputs();
            _controlledCharacter = controlledCharacter;
            _skillObservers = new Dictionary<SkillType, ISkillObserver>();
            _skillObservers.Add(SkillType.Directed, new DirectedSkillObserver(_camera));
            _skillObservers.Add(SkillType.RangeTargetZone, new RangeTargetZoneSkillObserver(_camera));
        }

        public NavMeshAgent NavigantionAgent => _navigation;

        public void Move()
        {
            var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
            {
                _navigation.speed = _controlledCharacter.Speed;
                _navigation.SetDestination(hit.point);
                _animationController.ChangeAnimation(AnimationType.Walk);
                if (!_isMoving)
                    ObserveMovingAnimation();
            }
        }

        private async UniTask ObserveMovingAnimation()
        {
            _isMoving = true;
            while (Math.Abs(_navigation.remainingDistance - _navigation.stoppingDistance) > 0.1f)
            {
                await UniTask.Delay(TimeSpan.FromMilliseconds(50));
            }

            _animationController.ChangeAnimation(AnimationType.Idle);
            _isMoving = false;
        }
        
        public void SetState(AnimationType animationType)
        {
            _animationController.ChangeAnimation(animationType);
        }

        public AnimatorStateInfo GetCurrentStateInfo()
        {
            return _animationController.GetCurrentStateInfo();
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