using System;

namespace Models
{
    public class Counter
    {
        private int _count = 0;
        
        public event Action<int> OnCountChanged;

        public void AddCount(int count)
        {
            _count += count;
            OnCountChanged?.Invoke(_count);
        }
    }
}