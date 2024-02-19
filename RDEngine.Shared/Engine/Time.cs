using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace RDEngine.Engine
{
    public class Time
    {
        public static Time Instance;

        public GameTime GameTime { get; set; }

        public float DeltaTime
        {
            get
            {
                return (float)GameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}