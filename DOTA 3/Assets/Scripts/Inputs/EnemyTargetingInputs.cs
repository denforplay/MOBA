using System;
using Models;
using UnityEngine;
using UnityEngine.InputSystem;
using Views;
using Views.UI;

namespace Inputs
{
    public class EnemyTargetingInputs
    {
        public event Action<TargetableView> OnTargetedEnemy;
        
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
                if (hit.collider.TryGetComponent(out TargetableView enemy))
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