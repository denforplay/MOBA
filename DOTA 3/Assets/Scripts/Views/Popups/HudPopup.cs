using Common.PopupSystem;

namespace Views.Popups
{
    public class HudPopup : Popup
    {
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