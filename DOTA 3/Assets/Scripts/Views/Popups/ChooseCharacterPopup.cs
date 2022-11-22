using System;
using System.Collections.Generic;
using System.Threading;
using Common.Enums;
using Common.PopupSystem;
using Configurations;
using Configurations.Character;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Views.UI.Panels;
using Zenject;
using CharacterInfo = Configurations.Character.CharacterInfo;

namespace Views.Popups
{
    public class ChooseCharacterPopup : Popup
    {
        private readonly string TIMER_TEMPLATE = "{0:D2}:{1:D2}";

        [SerializeField] private ChooseCharacterConfiguration _chooseCharacterConfiguration;
        [SerializeField] private CharactersConfiguration _charactersConfiguration;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private Button _finallyChooseCharacterButton;
        [SerializeField] private Button _chooseCharacterButtonPrefab;
        [SerializeField] private TeamCharactersPanel _leftTeamPanel;
        [SerializeField] private TeamCharactersPanel _rightTeamPanel;
        [SerializeField] private Image _choosePanel;
        [SerializeField] private ChooseCharacterInfoPanel _chooseCharacterInfoPanel;
        private TimeSpan _timer;
        private CharacterInfo _currentCharacterInfo;

        private CancellationTokenSource _chooseCancellationTokenSource;
        private PopupSystem _popupSystem;

        [Inject]
        public void Inject(PopupSystem popupSystem)
        {
            _popupSystem = popupSystem;
        }
        
        private void Awake()
        {
            _chooseCancellationTokenSource = new CancellationTokenSource();
            _finallyChooseCharacterButton.onClick.AddListener(ChooseCharacter);
            _finallyChooseCharacterButton.enabled = false;
        }

        private void ChooseCharacter()
        {
            _leftTeamPanel.CharactersImages[0].sprite = _currentCharacterInfo.CharacterIcon;
            _chooseCancellationTokenSource.Cancel();
        }

        private void Start()
        {
            foreach (var characterInfo in _charactersConfiguration.CharacterInfos)
            {
                CreateButtonFromCharacterInfo(characterInfo);
            }

            _timer += TimeSpan.FromSeconds(_chooseCharacterConfiguration.SecondsForChoose);
            UniTask.Create(StartTimer);
        }

        private void CreateButtonFromCharacterInfo(CharacterInfo characterInfo)
        {
            var button = Instantiate(_chooseCharacterButtonPrefab, _choosePanel.transform);
            button.image.sprite = characterInfo.CharacterIcon;
            button.onClick.AddListener(() => DisplayCharacterInfo(characterInfo));
        }

        private void DisplayCharacterInfo(CharacterInfo characterInfo)
        {
            _finallyChooseCharacterButton.enabled = true;
            _currentCharacterInfo = characterInfo;
            _chooseCharacterInfoPanel.DisplayCharacterInfo(characterInfo);
        }
        
        private async UniTask StartTimer()
        {
            var secondSpan = TimeSpan.FromSeconds(1);
            try
            {
                while (_timer.TotalSeconds > 0)
                {
                    _timerText.text = String.Format(TIMER_TEMPLATE, _timer.Minutes, _timer.Seconds);
                    await UniTask.Delay(secondSpan, cancellationToken: _chooseCancellationTokenSource.Token);
                    _timer -= secondSpan;
                }
            }
            finally
            {
                _timer = TimeSpan.Zero;
                _timer += TimeSpan.FromSeconds(_chooseCharacterConfiguration.SecondsBeforeStart);
                while (_timer.TotalSeconds > 0)
                {
                    _timerText.text = String.Format(TIMER_TEMPLATE, _timer.Minutes, _timer.Seconds);
                    await UniTask.Delay(secondSpan);
                    _timer -= secondSpan;
                }
                
                OnClosing();
                var popup = _popupSystem.SpawnPopup<GamePopup>(1);
                
                var leftTeam = new List<CharacterInfo>
                {
                    _currentCharacterInfo,
                    _charactersConfiguration.CharacterInfos[1],
                    _charactersConfiguration.CharacterInfos[0]
                };

                var rightTeam = new List<CharacterInfo>()
                {
                    _charactersConfiguration.CharacterInfos[1],
                    _charactersConfiguration.CharacterInfos[0],
                    _charactersConfiguration.CharacterInfos[0]
                };
                
                popup.Initialize(leftTeam, rightTeam);
            }

        }

        public override void EnableInput()
        {
        }

        public override void DisableInput()
        {
        }
    }
}