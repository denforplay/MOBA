using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class SkillView : MonoBehaviour
    {
        public event Action<int, bool> OnSkillUseStateChanged; 

        [SerializeField] private int _id;
        [SerializeField] private Image _skillImage;
        [SerializeField] private Image _darkImage;
        [SerializeField] private TextMeshProUGUI _countDownText;
        private CancellationTokenSource _cancellationToken;
        private float _countdownTime;
        
        public int Id => _id;
        public Image SkillImg => _skillImage;
        public Image DarkSkillImg => _darkImage;
        public CancellationTokenSource CancellationToken => _cancellationToken;

        public void SetCountdownTime(float countdownTime)
        {
            _countdownTime = countdownTime;
        }
        
        public async UniTask Countdown()
        {
            _cancellationToken?.Dispose();
            _cancellationToken = new CancellationTokenSource();
            OnSkillUseStateChanged?.Invoke(_id, false);
            _darkImage.fillAmount = 1;
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
            _cancellationToken.Cancel();
            OnSkillUseStateChanged?.Invoke(_id, true);
        }
        
        public bool IsSkillReadyToUse()
        {
            if (_cancellationToken is not null)
            {
                return _cancellationToken.IsCancellationRequested;
            }

            return true;
        }
    }
}