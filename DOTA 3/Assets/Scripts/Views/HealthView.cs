using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class HealthView : MonoBehaviour
    {
        private const string HEALTH_MATERIAL_VARIABLE = "_Health";
        private const string MAX_HEALTH_MATERIAL_VARIABLE = "_MaxHealth";
        [SerializeField] private GameObject _healthObject;
        [SerializeField] private Material _healthMaterialPrefab;
        [SerializeField] private Image _healthImage;
        
        private void Awake()
        {
            var newMaterial = Instantiate(_healthMaterialPrefab);
            _healthImage.material = newMaterial;
        }

        private void Update()
        {
            var position = _healthObject.transform.position;
            transform.position = new Vector3(position.x, transform.position.y, position.z);
        }

        public void SetMaxHealth(float value)
        {
            _healthImage.material.SetFloat(MAX_HEALTH_MATERIAL_VARIABLE, value);
        }

        public void SetHealth(float value)
        {
            _healthImage.material.SetFloat(HEALTH_MATERIAL_VARIABLE, value);
        }
    }
}