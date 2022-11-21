using System;
using Common.PopupSystem;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Views.Popups
{
    public class DeadPlayerPopup : Popup
    {
        public event Action OnRevive;
        
        private readonly string TIMER_TEMPLATE = "{0:D2}:{1:D2}";
        
        [SerializeField] private TextMeshProUGUI _timerText;

        public void Initialize(TimeSpan reviveTime)
        {
            UniTask.Create(() => StartReviving(reviveTime));
        }

        private async UniTask StartReviving(TimeSpan reviveTime)
        {
            var secondSpan = TimeSpan.FromSeconds(1);
            while (reviveTime.TotalSeconds > 0)
            {
                _timerText.text = String.Format(TIMER_TEMPLATE, reviveTime.Minutes, reviveTime.Seconds);
                reviveTime -= secondSpan;
                await UniTask.Delay(secondSpan);
            }
            
            OnRevive?.Invoke();
            OnClosing();
        }

        public override void EnableInput()
        {
        }

        public override void DisableInput()
        {
        }
    }
}