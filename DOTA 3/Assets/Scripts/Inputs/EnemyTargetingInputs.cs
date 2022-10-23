using System;
using Common.Abstracts;
using Models;
using UnityEngine;
using UnityEngine.InputSystem;
using Views;

namespace Inputs
{
    public class EnemyTargetingInputs
    {
        public event Action<EnemyView> OnTargetedEnemy;
        
        private Character _currentCharacter;
        private readonly Camera _camera;

        public EnemyTargetingInputs(Camera camera)
        {
            _camera = camera;
        }
        
        public void CheckTargetOnClick()
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Mouse.current.position.ReadValue()), out var hit, Mathf.Infinity))
            {
                if (hit.collider.TryGetComponent(out EnemyView enemy))
                {
                    OnTargetedEnemy?.Invoke(enemy);
                }
                else
                {
                    OnTargetedEnemy?.Invoke(null);
                }
            }
        }
    }
}