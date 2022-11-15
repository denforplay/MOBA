using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Views.UI.Panels
{
    public class GameInfoPanel : MonoBehaviour
    {
        private readonly string TIMER_TEMPLATE = "{0:D2}:{1:D2}";
        [SerializeField] private TextMeshProUGUI _timerText;
        private TimeSpan _timer;
        private void Awake()
        {
            UniTask.Create(StartTimer);
        }

        private async UniTask StartTimer()
        {
            while (true)
            {
                var secondSpan = TimeSpan.FromSeconds(1);
                await UniTask.Delay(secondSpan);
                _timer += secondSpan;
                _timerText.text = String.Format(TIMER_TEMPLATE, _timer.Minutes, _timer.Seconds);
            }
        }
    }
}