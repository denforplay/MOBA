using Common.Abstracts;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class ManaView : MonoBehaviour
    {
        private const string MANA_MATERIAL_VARIABLE = "_Mana";
        private const string MAX_MANA_MATERIAL_VARIABLE = "_MaxMana";
        [SerializeField] private GameObject _manaObject;
        [SerializeField] private Material _manaMaterialPrefab;
        [SerializeField] private Image _manaImage;
        private IManable _manable;

        public void AttachManaModel(IManable manable)
        {
            var newMaterial = new Material(_manaMaterialPrefab);
            _manaImage.material = newMaterial;
            _manable = manable;
            _manable.OnManaChanged += SetMana;
            _manable.OnMaxManaChanged += SetMaxMana;
            SetMana(_manable.CurrentMana);
            SetMaxMana(_manable.MaxMana);
        }

        private void Update()
        {
            var position = _manaObject.transform.position;
            transform.position = new Vector3(position.x, transform.position.y, position.z);
        }

        public void SetMaxMana(float value)
        {
            _manaImage.material.SetFloat(MAX_MANA_MATERIAL_VARIABLE, value);
        }

        public void SetMana(float value)
        {
            _manaImage.material.SetFloat(MANA_MATERIAL_VARIABLE, value);
        }
    }
}