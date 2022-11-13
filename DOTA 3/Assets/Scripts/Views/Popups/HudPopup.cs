using Common.PopupSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Views.UI.Panels;

namespace Views.Popups
{
    public class HudPopup : Popup
    {
        [SerializeField] private HeroPanel _heroPanel;
        [SerializeField] private Image _heroImage;
        [SerializeField] private TextMeshProUGUI _heroName;
        [SerializeField] private TextMeshProUGUI _coinText;
        
        public void Initialize(CharacterView characterView)
        {
            _heroPanel.SetSkills(characterView.Skills);
            characterView.OnSkillActivated += _heroPanel.ResetSkill;
            characterView.SubscribeOnMoneyChanged((money) => _coinText.text = money.ToString());
            foreach (var skillView in _heroPanel.SkillViews)
            {
                skillView.OnSkillUseStateChanged += characterView.SetSkillUseState;
            }
        }
        
        public override void EnableInput()
        {
            enabled = true;
        }

        public override void DisableInput()
        {
            enabled = false;
        }
    }
}