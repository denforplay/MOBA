using System;

namespace Common.Abstracts
{
    public interface ILevelable
    {
        event Action<int> OnLevelChanged;
        event Action<float> OnCurrentExperienceChanged;
        event Action<float> OnNeedForNextLevelExperienceChanged; 
        int CurrentLevel { get; }
        float CurrentExperience { get; }
        float NeedForNextLevelExperience { get; }
        void AddExperience(float value);
    }
}