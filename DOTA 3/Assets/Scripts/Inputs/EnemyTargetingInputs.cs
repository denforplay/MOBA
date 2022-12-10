using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Views;

namespace Inputs
{
    public class EnemyTargetingInputs
    {
        public event Action<TargetableView> OnTargetedEnemy;
        
        private CharacterView _currentCharacter;
        private readonly Camera _camera;

        public EnemyTargetingInputs(Camera camera, CharacterView characterView)
        {
            _currentCharacter = characterView;
            _camera = camera;
        }
        
        public void CheckTargetOnClick()
        {
            Debug.Log("click somewhere");
            if (Physics.Raycast(_camera.ScreenPointToRay(Mouse.current.position.ReadValue()), out var hit, Mathf.Infinity));
            {
                if (hit.collider.TryGetComponent(out TargetableView enemy) && enemy.Team != _currentCharacter.Team)
                {
                    Debug.Log("enemy detected");
                    OnTargetedEnemy?.Invoke(enemy);
                }
                else
                {
                    Debug.Log("enemy not detected");
                    OnTargetedEnemy?.Invoke(null);
                }
            }
        }
    }
}