using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RDEngine.Engine
{
    public class GComponent
    {
        public GameObject Parent;
        public bool Enabled = true;

        public static bool ShowHitboxes;

        public GComponent()
        {
            
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