using System;
using UnityEngine;
using UnityEngine.UI;
using Views.UI.Panels;

namespace Views.UI.Images
{
    public class CharacterBattlegroundInfoImage : MonoBehaviour
    {
        [SerializeField] private Image _playerImage;
        [SerializeField] private TimerPanel _timerPanel;
        public Image PlayerImage => _playerImage;
    }
}