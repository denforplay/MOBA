using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Configurations;
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
        private float _startTime;
        private float _countdownTime;
        private SkillConfiguration _skillConfiguration;
        private bool _isManaEnough = true;

        public SkillConfiguration SkillConfiguration => _skillConfiguration;

        public int Id => _id;
        public Image SkillImg => _skillImage;
        public Image DarkSkillImg => _darkImage;
        public CancellationTokenSource CancellationToken => _cancellationToken;

        public void SetSkillConfiguration(SkillConfiguration skillConfiguration)
        {
            _skillConfiguration = skillConfiguration;
        }

        public void DisableOnManaNotEnough()
        {
            _isManaEnough = false;
            _darkImage.fillAmount = 1;
        }

        public void EnableOnManaEnough()
        {
            _isManaEnough = true;
            
            if (_cancellationToken.IsCancellationRequested)
            {
                _darkImage.fillAmount = 1;
            }
            else 
            {
                if (_countdownTime != _startTime)
                {
                    _darkImage.fillAmount = (_startTime - _countdownTime) / _startTime;
                }
                else
                {
                    _darkImage.fillAmount = 1;
                }
            }
        }
        
        public void SetCountdownTime(float countdownTime)
        {
            _startTime = countdownTime;
            _countdownTime = countdownTime;

        }
        
        public async UniTask Countdown()
        {
            _cancellationToken?.Dispose();
            _cancellationToken = new CancellationTokenSource();
            OnSkillUseStateChanged?.Invoke(_id, false);
            _darkImage.fillAmount = 1;
            var reduceAmountPerTick = 1 / (_countdownTime * 10);
            while (_countdownTime > 0)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.1));
                _countDownText.text = _countdownTime.ToString("0.0");
                if (_isManaEnough)
                    _darkImage.fillAmount -= reduceAmountPerTick;
                _countdownTime -= 0.1f;
            }

            _countdownTime = _startTime;
            _countDownText.text = "";
            _cancellationToken.Cancel();
            if (_isManaEnough)
                OnSkillUseStateChanged?.Invoke(_id, true);
        }
    }
}