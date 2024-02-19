using Microsoft.Xna.Framework;

namespace RDEngine.Engine
{
    public struct RectF
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public RectF(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }
    }
}