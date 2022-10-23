﻿using System;
using System.Collections.Generic;
using Configurations;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI.Panels
{
    public class HeroPanel : MonoBehaviour
    {
        [SerializeField] private Image _heroPanel;
        [SerializeField] private SkillPanel _skillPanel;

        public void SetSkills(IEnumerable<SkillConfiguration> skillConfigurations)
        {
            foreach (var skillConfig in skillConfigurations)
            {
                _skillPanel.SetSkill(skillConfig);
            }
        }

        public List<SkillView> SkillViews => _skillPanel.SkillViews;

        public void ResetSkill(int skillId)
        {
            _skillPanel.ResetSkill(skillId);
        }

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }
    }
}