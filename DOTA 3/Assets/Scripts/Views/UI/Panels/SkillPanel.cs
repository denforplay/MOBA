using System;
using System.Collections.Generic;
using System.Linq;
using Configurations;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Views.UI.Images;

namespace Views.UI.Panels
{
    public class SkillPanel : MonoBehaviour
    {
        [SerializeField] private Image _panel;
        [SerializeField] private List<SkillImage> _skillImages;

        public void SetSkill(SkillConfiguration skillConfig)
        {
            var skillImage = _skillImages.FirstOrDefault(x => x.Id == skillConfig.Id);
            if (skillImage is not null)
            {
                skillImage.SkillImg.sprite = skillConfig.SkillSprite;
                skillImage.DarkSkillImg.sprite = skillConfig.SkillSprite;
                skillImage.SetCountdownTime(skillConfig.CountDowntime);
                UniTask.Create(skillImage.Countdown);
            }
        }

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }
    }
}
