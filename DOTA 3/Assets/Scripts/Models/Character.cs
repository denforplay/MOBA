using Common.Abstracts;
using Configurations;

namespace Models
{
    public class Character : IMovable
    {
        public Character(CharacterInfo characterInfo)
        {
            Speed = characterInfo.Speed;
        }
        
        public float Speed { get; set; }
    }
}
