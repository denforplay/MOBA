using System;
using Cysharp.Threading.Tasks;

namespace Models
{
    public class Timer
    {
        private TimeSpan _time;
        public event Action OnTimerEnded;
        public event Action<TimeSpan> OnTimerTick;

        public Timer(TimeSpan time)
        {
            _time = time;
        }

        public async UniTask StartTimer()
        {
            var secondSpan = TimeSpan.FromSeconds(1);
            
            while (_time.TotalSeconds > 0)
            {
                OnTimerTick?.Invoke(_time);
                await UniTask.Delay(secondSpan);
                _time -= secondSpan;
            }

            OnTimerEnded?.Invoke();
        }
    }
}