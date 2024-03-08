using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RDEngine.Engine
{
    public class GComponent
    {
        public GameObject Parent;
        private bool _enabled;
        public bool Enabled
        {
            get
            {
                return _enabled && Parent.Enabled;
            }
            set
            {
                _enabled = value;
            }
        }

        public static bool ShowHitboxes;

        internal static Texture2D WhitePixel;

        public GComponent()
        {
            Enabled = true;
        }

        public virtual void SetParent(GameObject parent)
        {
            Parent = parent;
        }

        public virtual void Start()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void LateUpdate()
        {

        }

        public virtual void FixedUpdate()
        {
            throw new System.Exception("Feature not implemented yet.");
        }

        public virtual void Draw(GraphicsDevice graphics, SpriteBatch spriteBatch)
        {

        }

        public virtual void Remove()
        {
            Parent.RemoveComponent(this);
        }
    }
}