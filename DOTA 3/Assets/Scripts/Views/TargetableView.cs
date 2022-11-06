using Common.Enums;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class TargetableView : MonoBehaviour
    {
        [SerializeField] private Image _targetMark;

        private Team _team;

        public Team Team => _team;
        
        private void Awake()
        {
            _targetMark.enabled = false;
        }

        public void SetTeam(Team team)
        {
            _team = team;
        }
        
        public void SetAsTarget(bool isTarget)
        {
            _targetMark.enabled = isTarget;
            _targetMark.gameObject.SetActive(isTarget);
        }
    }
}