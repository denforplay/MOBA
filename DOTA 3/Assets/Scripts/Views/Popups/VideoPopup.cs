using System;
using Common.PopupSystem;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Views.Popups
{
    public class VideoPopup : Popup
    {
        public event Action OnVideoEnded;
        [SerializeField] private VideoPlayer _videoPlayer;
        [SerializeField] private Button _skipButton;

        private void Awake()
        {
            _videoPlayer.Play();
            _skipButton.onClick.AddListener(SkipVideo);
            _videoPlayer.loopPointReached += _ => OnVideoEnded?.Invoke();
        }

        private void SkipVideo()
        {
            _videoPlayer.Stop();
            OnVideoEnded?.Invoke();
        }

        public override void EnableInput()
        {
        }

        public override void DisableInput()
        {
        }
    }
}