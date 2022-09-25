using System;
using UnityEngine;
using Zenject;

namespace Common.PopupSystem
{
    public abstract class Popup : MonoBehaviour
    {
        public event Action<Popup> Closing;

        public virtual void Show()
        {
        }

        public virtual void Hide()
        {
            Destroy(gameObject);
        }

        public abstract void EnableInput();
        public abstract void DisableInput();

        protected void OnClosing()
        {
            Closing?.Invoke(this);
        }
    }
}