using System;

namespace Common.Abstracts
{
    public interface IManable
    {
        event Action<float> OnMaxManaChanged;
        event Action<float> OnManaChanged;
        float MaxMana { get; }
        float CurrentMana { get; }
        void ChangeMana(float value);
    }
}