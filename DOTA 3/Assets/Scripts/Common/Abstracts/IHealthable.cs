using System;

namespace Common.Abstracts
{
    public interface IHealthable
    {
        event Action<float> OnMaxHealthChanged;
        event Action<float> OnHealthChanged;
        event Action OnHealthEnded;
        float MaxHealth { get; }
        float CurrentHealth { get; }
        void ChangeHealth(float value);
    }
}