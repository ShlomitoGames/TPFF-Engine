using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RDEngine.Engine.Physics;
using System.Collections.Generic;

namespace RDEngine.Engine
{
    public class WorldObject : GameObject
    {
        public Vector2 WorldPosition //Measured in units
        {
            get
            {
                return Position / Scene.UnitSize;
            }
            set
            {
                Position = value * Scene.UnitSize;
            }
        }

        public WorldObject(string tag, Texture2D texture, Vector2 worldPos, List<GComponent> initialComponents = null, List<WorldObject> children = null) : base(tag, texture, Vector2.Zero, initialComponents, (children != null) ? children.ConvertAll(x => x as GameObject) : null)
        {
            WorldPosition = worldPos;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (Texture == null) return;

            Vector2 pos = AbsolutePos - Scene.WorldCameraPos - (new Vector2(Texture.Width, Texture.Height) * Scale / 2f) + Vector2.One * 2f;
            spriteBatch.Draw(Texture, Vector2.Floor(pos), null, Color, 0f, Vector2.Zero, Scale, Effects, LayerDepth);
        }

        internal void OnCollisionEnter(Collision collision)
        {
            foreach (var component in _components)
            {
                var collideable = component as ICollideable;
                if (collideable == null) continue;

                collideable.OnCollisionEnter(collision);
            }
        }
        internal void OnCollisionStay(Collision collision)
        {
            foreach (var component in _components)
            {
                var collideable = component as ICollideable;
                if (collideable == null) continue;

                collideable.OnCollisionStay(collision);
            }
        }
        internal void OnCollisionExit(Collision collision)
        {
            foreach (var component in _components)
            {
                var collideable = component as ICollideable;
                if (collideable == null) continue;

                collideable.OnCollisionExit(collision);
            }
        }

        internal void OnTriggerEnter(RigidBody intrRb)
        {
            foreach (var component in _components)
            {
                var collideable = component as ICollideable;
                if (collideable == null) continue;

                collideable.OnTriggerEnter(intrRb);
            }
        }
        internal void OnTriggerStay(RigidBody intrRb)
        {
            foreach (var component in _components)
            {
                var collideable = component as ICollideable;
                if (collideable == null) continue;

                collideable.OnTriggerStay(intrRb);
            }
        }
        internal void OnTriggerExit(RigidBody intrRb)
        {
            foreach (var component in _components)
            {
                var collideable = component as ICollideable;
                if (collideable == null) continue;

                collideable.OnTriggerExit(intrRb);
            }
        }
    }
}