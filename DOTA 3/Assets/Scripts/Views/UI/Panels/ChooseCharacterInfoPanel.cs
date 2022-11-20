using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CharacterInfo = Configurations.Character.CharacterInfo;

namespace Views.UI.Panels
{
    public class ChooseCharacterInfoPanel : MonoBehaviour
    {
        [SerializeField] private Image _choosedCharacterImage;
        [SerializeField] private TextMeshProUGUI _strengthText;
        [SerializeField] private TextMeshProUGUI _defenseText;
        [SerializeField] private TextMeshProUGUI _agilityText;
        [SerializeField] private TextMeshProUGUI _intelligenceText;

        public void DisplayCharacterInfo(CharacterInfo characterInfo)
        {
            
        }
    }
}