using System;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class HealthView : MonoBehaviour
    {
        public const string HEALTH_MATERIAL_VARIABLE = "_Health";
        [SerializeField] private CharacterView _characterView;
        [SerializeField] private Material _healthDisplayMaterial;

        private void Update()
        {
            var position = _characterView.transform.position;
            transform.position = new Vector3(position.x, transform.position.y, position.z);
        }

        public void SetHealth(int value)
        {
            _healthDisplayMaterial.SetFloat(HEALTH_MATERIAL_VARIABLE, value);
        }
    }
}