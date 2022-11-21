using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CharacterInfo = Configurations.Character.CharacterInfo;

namespace Views.UI.Panels
{
    public class ChooseCharacterInfoPanel : MonoBehaviour
    {
        [SerializeField] private Image _choosedCharacterImage;
        [SerializeField] private TextMeshProUGUI _charaterName;
        [SerializeField] private TextMeshProUGUI _characterDescription;
        [SerializeField] private TextMeshProUGUI _strengthText;
        [SerializeField] private TextMeshProUGUI _defenseText;
        [SerializeField] private TextMeshProUGUI _intelligenceText;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void DisplayCharacterInfo(CharacterInfo characterInfo)
        {
            _choosedCharacterImage.sprite = characterInfo.CharacterIcon;
            _charaterName.text = characterInfo.HeroName;
            _characterDescription.text = characterInfo.HeroDescription;
            _strengthText.text = characterInfo.BaseStrength.ToString();
            _defenseText.text = characterInfo.BaseDefense.ToString();
            _intelligenceText.text = characterInfo.BaseIntelligence.ToString();
            gameObject.SetActive(true);
        }
    }
}