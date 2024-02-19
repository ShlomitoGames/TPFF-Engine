﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RDEngine.Engine.Physics;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;

namespace RDEngine.Engine
{
    public class WorldObject : GameObject
    {
        public WorldObject(string tag, Scene scene, Texture2D texture, Vector2 worldPos, GameObject parent = null, List<GComponent> initialComponents = null) : base(tag, scene, texture, worldPos * 16f, parent, initialComponents)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (_texture == null) return;

            spriteBatch.Draw(_texture, Vector2.Floor(Position - Scene.WorldCameraPos + Vector2.One), null, Color, 0f, Vector2.Zero, Vector2.One, Effects, Layer);
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