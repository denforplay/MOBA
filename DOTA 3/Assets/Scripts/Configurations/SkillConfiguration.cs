﻿using Common.Enums;
using UnityEngine;

namespace Configurations
{
    [CreateAssetMenu(menuName = "Configurations/Skill configuration", order = 0)]
    public class SkillConfiguration : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private Sprite _skillSprite;
        [SerializeField] private float _countdownTime;
        [SerializeField] private SkillType _skillType;
        [SerializeField] private AnimationClip _skillAnimation;
        [SerializeField] private float _effectValue;
        [SerializeField] private float _range;
        [SerializeField] private float _manaCost;
        [SerializeField] private ParticleSystem _particle;
        
        public int Id => _id;
        public Sprite SkillSprite => _skillSprite;
        public float CountDowntime => _countdownTime;
        public SkillType SkillType => _skillType;
        public AnimationClip SkillAnimation => _skillAnimation;
        public float EffectValue => _effectValue;
        public float Range => _range;
        public ParticleSystem ParticleSystem => _particle;
        public float ManaCost => _manaCost;
    }
}