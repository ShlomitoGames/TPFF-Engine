using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace RDEngine.Engine
{
    public static class Time
    {
        public static GameTime GameTime { get; set; }

        public static float DeltaTime
        {
            get
            {
                return (float)GameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}