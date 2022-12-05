using System;
using System.Collections.Generic;
using Configurations;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI.Panels
{
    public class HeroPanel : MonoBehaviour
    {
        [SerializeField] private Image _heroImage;
        [SerializeField] private SkillPanel _skillPanel;
        [SerializeField] private Slider _healthBar;
        [SerializeField] private Slider _manaBar;
        [SerializeField] private TextMeshProUGUI _strengthText;
        [SerializeField] private TextMeshProUGUI _intelligenceText;
        [SerializeField] private TextMeshProUGUI _defenseText;
        [SerializeField] private TextMeshProUGUI _levelText;

        public void Initialize(Character character)
        {
            _healthBar.maxValue = character.MaxHealth;
            _manaBar.maxValue = character.MaxMana;
            _healthBar.value = character.CurrentHealth;
            _manaBar.value = character.CurrentMana;
        }

        public void SetCurrentMana(float mana)
        {
            _manaBar.value = mana;
        }

        public void SetCurrentMaxMana(float maxMana)
        {
            _manaBar.maxValue = maxMana;
        }

        public void SetCurrentHealth(float health)
        {
            _healthBar.value = health;
        }

        public void SetMaxHealth(float maxHealth)
        {
            _healthBar.maxValue = maxHealth;
        }

        public void SetSkills(IEnumerable<SkillConfiguration> skillConfigurations)
        {
            foreach (var skillConfig in skillConfigurations)
            {
                _skillPanel.SetSkill(skillConfig);
            }
        }

        public void SetCharacterIcon(Sprite sprite)
        {
            _heroImage.sprite = sprite;
        }

        public void SetCharacterStrength(int value)
        {
            _strengthText.text = value.ToString();
        }

        public void SetCharacterIntelligence(int value)
        {
            _intelligenceText.text = value.ToString();
        }

        public void SetCharacterDefense(int value)
        {
            _defenseText.text = value.ToString();
        }

        public void SetCharacterLevel(int value)
        {
            _levelText.text = value.ToString();
        }

        public List<SkillView> SkillViews => _skillPanel.SkillViews;

        public void ResetSkill(int skillId)
        {
            _skillPanel.ResetSkill(skillId);
        }
    }
}