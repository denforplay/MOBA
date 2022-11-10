using System;

namespace Common.Abstracts
{
    public interface IHealthable
    {
        float MaxHealth { get; }
        float CurrentHealth { get; }
        event Action<float> OnHealthChanged;
        event Action OnHealthEnded;
        void ChangeHealth(float value);
    }
}