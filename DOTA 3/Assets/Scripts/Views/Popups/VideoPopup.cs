using System;
using Common.PopupSystem;
using UnityEngine;
using UnityEngine.Video;

namespace Views.Popups
{
    public class VideoPopup : Popup
    {
        public event Action OnVideoEnded;
        [SerializeField] private VideoPlayer _videoPlayer;

        private void Awake()
        {
            _videoPlayer.Play();
            _videoPlayer.loopPointReached += _ => OnVideoEnded?.Invoke();
        }

        public override void EnableInput()
        {
        }

        public override void DisableInput()
        {
        }
    }
}