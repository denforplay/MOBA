using System;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private Image _targetMark;

        private void Awake()
        {
            _targetMark.enabled = false;
        }

        public void SetAsTarget(bool isTarget)
        {
            _targetMark.enabled = isTarget;
            _targetMark.gameObject.SetActive(isTarget);
        }
    }
}