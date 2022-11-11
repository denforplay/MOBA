using System;
using System.Threading;
using Common.Abstracts;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Models.Skills.Observers
{
    public class DirectedSkillObserver : ISkillObserver
    {
        public event Action<Vector3> OnObservedPositionChanged;
        private readonly Camera _camera;

        public DirectedSkillObserver(Camera camera)
        {
            _camera = camera;
        }

        public async UniTask ObserveSkill(CancellationTokenSource cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var mousePosition = Mouse.current.position.ReadValue();
                Ray ray = _camera.ScreenPointToRay(mousePosition);
                var layerMask = LayerMask.GetMask("Terrain");
                if (Physics.Raycast(ray, out var raycastHit, Mathf.Infinity, layerMask))
                {
                    var skillPosition = new Vector3(raycastHit.point.x, raycastHit.point.y, raycastHit.point.z);
                    OnObservedPositionChanged?.Invoke(skillPosition);
                }
                
                await UniTask.Delay(TimeSpan.FromSeconds(0.01f));
            }
        }
    }
}