using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI.Buttons
{
    public class BuySellButton : MonoBehaviour
    {
        [SerializeField] private Button _buyButton;
        [SerializeField] private TextMeshProUGUI _buttonText;

        public void SetText(string text)
        {
            _buttonText.text = text;
        }
        
        public Button Button => _buyButton;
    }
}