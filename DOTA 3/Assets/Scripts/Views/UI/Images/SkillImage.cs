using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI.Images
{
    public class SkillImage : MonoBehaviour
    {
        [SerializeField] private int _id;
        [SerializeField] private Image _skillImage;
        [SerializeField] private Image _darkImage;
        [SerializeField] private TextMeshProUGUI _countDownText;
        
        private float _countdownTime;
        
        public int Id => _id;
        public Image SkillImg => _skillImage;
        public Image DarkSkillImg => _darkImage;

        public void SetCountdownTime(float countdownTime)
        {
            _countdownTime = countdownTime;
        }
        
        public async UniTask Countdown()
        {
            var reduceAmountPerTick = 1 / (_countdownTime * 10);
            float startTime = _countdownTime;
            while (_countdownTime > 0)
            {
                _countDownText.text = _countdownTime.ToString("0.0");
                _darkImage.fillAmount -= reduceAmountPerTick;
                await UniTask.Delay(TimeSpan.FromSeconds(0.1));
                _countdownTime -= 0.1f;
            }

            _countdownTime = startTime;
            _countDownText.text = "";
        }
    }
}