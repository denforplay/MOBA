using Common.PopupSystem;
using UnityEngine;
using Views.UI.Panels;

namespace Views.Popups
{
    public class HudPopup : Popup
    {
        [SerializeField] private HeroPanel _heroPanel;
        
        public void Initialize(CharacterView characterView)
        {
            _heroPanel.SetSkills(characterView.Skills);
            characterView.OnSkillActivated += _heroPanel.ResetSkill;
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