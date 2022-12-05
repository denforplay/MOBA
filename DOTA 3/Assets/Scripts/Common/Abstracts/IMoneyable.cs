using System;

namespace Assets.Scripts.Common.Abstracts
{
    public interface IMoneyable
    {
        public event Action<int> OnMoneyChanged;

        public int Money { get; set; }
    }
}
