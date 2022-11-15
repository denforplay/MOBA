using Common.Abstracts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class LevelView : MonoBehaviour
    {
        private const string EXPERIENCE_MATERIAL_VARIABLE = "_Experience";
        private const string MAX_EXPERIENCE_MATERIAL_VARIABLE = "_MaxExperience";
        
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private GameObject _experiencedObject;
        [SerializeField] private Material _experienceMaterialPrefab;
        [SerializeField] private Image _experienceBar;
        private ILevelable _levelable;

        public void AttachLevelableModel(ILevelable levelable)
        {
            var newMaterial = new Material(_experienceMaterialPrefab);
            _experienceBar.material = newMaterial;
            
            _levelable = levelable;
            _levelable.OnLevelChanged += SetLevel;
            _levelable.OnCurrentExperienceChanged += SetCurrentExperience;
            _levelable.OnNeedForNextLevelExperienceChanged += SetNextLevelExperience;
            SetLevel(_levelable.CurrentLevel);
            SetCurrentExperience(_levelable.CurrentExperience);
            SetNextLevelExperience(_levelable.NeedForNextLevelExperience);
        }
        
        private void Update()
        {
            var position = _experiencedObject.transform.position;
            transform.position = new Vector3(position.x, transform.position.y, position.z);
        }
        
        public void SetLevel(int level)
        {
            _levelText.text = level.ToString();
        }

        public void SetCurrentExperience(float value)
        {
            _experienceBar.material.SetFloat(EXPERIENCE_MATERIAL_VARIABLE, value);
        }

        public void SetNextLevelExperience(float value)
        {
            _experienceBar.material.SetFloat(MAX_EXPERIENCE_MATERIAL_VARIABLE, value);
        }
    }
}