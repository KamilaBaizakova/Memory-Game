using System;

namespace Memory_Game
{
    public class Player
    {
        public string Username { get; set; }
        public int Mistakes { get; set; }
        public string ElapsedTime { get; set; }
        public TimeSpan EffectiveTime { get; set; }
    }
}
