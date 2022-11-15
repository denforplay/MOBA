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
        private bool _isRecreated;
        
        private void Start()
        {
            if (!_isRecreated)
                RecreateMaterial();
        }

        private void RecreateMaterial()
        {
            var newMaterial = new Material(_healthMaterialPrefab);
            _healthImage.material = newMaterial;
            _isRecreated = true;
        }

        private void Update()
        {
            var position = _healthObject.transform.position;
            transform.position = new Vector3(position.x, transform.position.y, position.z);
        }

        public void SetMaxHealth(float value)
        {
            if (!_isRecreated)
                RecreateMaterial();
            
            _healthImage.material.SetFloat(MAX_HEALTH_MATERIAL_VARIABLE, value);
        }

        public void SetHealth(float value)
        {
            if (!_isRecreated)
                RecreateMaterial();
            
            _healthImage.material.SetFloat(HEALTH_MATERIAL_VARIABLE, value);
        }
    }
}