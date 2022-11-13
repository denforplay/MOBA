using System;
using System.Threading;
using Common.Abstracts;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Models.Skills.Observers
{
    public class RangeTargetZoneSkillObserver : ISkillObserver
    {
        private readonly Camera _camera;
        public event Action<Vector3> OnObservedPositionChanged;

        public RangeTargetZoneSkillObserver(Camera camera)
        {
            _camera = camera;
        }
        
        public async UniTask ObserveSkill(CancellationTokenSource cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var mousePosition = Mouse.current.position.ReadValue();
                Ray ray = _camera.ScreenPointToRay(mousePosition);
                var mask = LayerMask.GetMask("Terrain");
                if (Physics.Raycast(ray, out var raycastHit, Mathf.Infinity, mask))
                {
                    var skillPosition = new Vector3(raycastHit.point.x, 0.15f, raycastHit.point.z);
                    OnObservedPositionChanged?.Invoke(skillPosition);
                }
                
                await UniTask.Delay(TimeSpan.FromSeconds(0.01f));
            }
        }
    }
}