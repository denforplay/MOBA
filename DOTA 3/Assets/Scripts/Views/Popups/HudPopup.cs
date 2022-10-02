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