using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace RDEngine.Engine
{
    public class GameObject
    {
        private Texture2D _texture;
        public Texture2D Texture
        {
            get
            {
                return _texture;
            }
            set
            {
                _texture = value;
                if (value != null)
                {
                    _originOffset = Texture.Bounds.Size.ToVector2() / 2f;
                }
            }
        }

        public Vector2 Position { get; set; }
        public Color Color;
        public SpriteEffects Effects;
        public float Layer { get; set; }

        public GameObject Parent;
        public Scene Scene;

        public string Tag;

        protected List<GComponent> _components;

        public Vector2 WorldPos
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
        private Vector2 _originOffset;
        public Vector2 Origin
        {
            get
            {
                return Position + _originOffset;
            }
        }

        public GameObject(string tag, Scene scene, Texture2D texture, Vector2 position, GameObject parent = null, List<GComponent> initialComponents = null)
        {
            Scene = scene;
            Texture = texture;
            Position = position;
            Tag = tag;
            Parent = parent;
            Effects = SpriteEffects.None;
            Color = Color.White;
            _originOffset = Vector2.Zero;

            if (initialComponents != null)
                _components = initialComponents;
            else
                _components = new List<GComponent>();

            foreach (var component in _components)
            {
                component.SetParent(this);
            }
        }

        internal void Start()
        {
            foreach (var component in _components)
            { 
                component.Start();
            }
        }
        internal void Update()
        {
            foreach (var component in _components)
            {
                if (component.Enabled)
                    component.Update();
            }
        }
        internal void LateUpdate()
        {
            foreach (var component in _components)
            {
                if (component.Enabled)
                    component.LateUpdate();
            }
        }
        internal virtual void FixedUpdate()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public T GetComponent<T>()
        {
            foreach (var component in _components)
            {
                if (typeof(T) == component.GetType())
                    return (T)(object)component;
            }
            return default;
        }

        internal void DrawComponents(GraphicsDevice graphics, SpriteBatch spriteBatch)
        {
            if (!GComponent.ShowHitboxes)
                return;

            foreach (var component in _components)
            {
                component.Draw(graphics, spriteBatch);
            }
        }

        public void AddComponent(GComponent component)
        {
            _components.Add(component);
            component.SetParent(this);
        }

        public void RemoveComponent(GComponent component)
        {
            _components.Remove(component);
        }

        public virtual void Destroy()
        {
            foreach (var component in _components)
            {
                RemoveComponent(component);
            }

            Scene.RemoveGameObject(this);
        }
    }
}