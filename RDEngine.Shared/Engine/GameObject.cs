using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace RDEngine.Engine
{
    public class GameObject
    {
        public Texture2D Texture;

        //Size relative to texture size
        public Vector2 Scale { get; set; } = Vector2.One;

        public Color Color;
        public SpriteEffects Effects;
        public float LayerDepth = 0f;

        public GameObject Parent { get; private set; }
        public Scene Scene;

        public string Tag;

        protected List<GameObject> _children;
        protected List<GComponent> _components;

        //Measured in RenderTarget pixels
        public Vector2 Position { get; set; }
        public Vector2 AbsolutePos
        {
            get
            {
                return (Parent != null) ? Position + Parent.Position : Position;
            }
            set
            {
                Position = ((Parent != null) ? value - Parent.Position : value);
            }
        }

        public bool Enabled;
        
        public GameObject(string tag, Texture2D texture, Vector2 position, List<GComponent> initialComponents = null, List<GameObject> children = null)
        {
            Scene = SceneHandler.ActiveScene;
            Texture = texture;
            Position = position;
            Tag = tag;
            Effects = SpriteEffects.None;
            Color = Color.White;
            Enabled = true;

            if (initialComponents != null)
                _components = initialComponents;
            else
                _components = new List<GComponent>();

            foreach (var component in _components)
            {
                component.SetParent(this);
            }

            if (children != null)
                _children = children;
            else
                _children = new List<GameObject>();

            foreach (var child in _children)
            {
                child.SetParent(this);
                Scene.AddGameObject(child);
            }
        }

        internal void Start()
        {
            if (!Enabled) return;

            foreach (var component in _components)
            {
                component.Start();
            }
        }
        internal void Update()
        {
            if (!Enabled) return;

            foreach (var component in _components)
            {
                if (component.Enabled)
                    component.Update();
            }
        }
        internal void LateUpdate()
        {
            if (!Enabled) return;

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
        internal void DrawComponents(GraphicsDevice graphics, SpriteBatch spriteBatch)
        {
            if (!GComponent.ShowHitboxes)
                return;

            foreach (var component in _components)
            {
                component.Draw(graphics, spriteBatch);
            }
        }

        public void SetParent(GameObject parent)
        {
            Parent = parent;
        }

        public GameObject[] GetChildren()
        {
            return _children.ToArray();
        }

        public GameObject GetChild(string tag)
        {
            foreach (var child in _children)
            {
                if (child.Tag == tag)
                    return child;
            }
            return null;
        }
        public GameObject GetChild()
        {
            if (_children.Count > 0)
                return _children[0];

            return null;
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
        //Returns first instance of the specified component in a child GameObject
        public T GetComponentInChildren<T>()
        {
            foreach (var child in _children)
            {
                return child.GetComponent<T>();
            }
            return default;
        }
        //Returns first instance of the specified component in all child GameObjects
        public T[] GetComponentsInChildren<T>()
        {
            List<T> result = new List<T>();

            foreach (var child in _children)
            {
                T component = child.GetComponent<T>();
                if (component != null)
                    result.Add(component);
            }
            return result.ToArray();
        }
        public T GetComponentInParent<T>()
        {
            return Parent.GetComponent<T>();
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