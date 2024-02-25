using RDEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RDEngine.Engine.Physics;
using System.Collections.Generic;
using RDEngine.Engine.UI;

namespace RDEngine.Engine
{
    public class Scene
    {
        protected List<WorldObject> _gameObjects;
        protected List<UIObject> _uiObjects;

        private byte _unitSize;
        public byte UnitSize { get { return _unitSize; } }

        public Vector2 CameraPos { get; set; }
        public Vector2 WorldCameraPos
        {
            get
            {
                return Vector2.Floor(CameraPos / RDEGame.ScaleFactor);
            }
        }

        public PhysicsSolver Solver;

        private bool _isFirstFrame;

        public Scene(byte unitSize = 16)
        {
            Solver = new PhysicsSolver();
            _unitSize = unitSize;
        }

        public virtual void Initialize(ContentManager content)
        {
            Solver = new PhysicsSolver();
            _gameObjects = null;
            _uiObjects = null;
            _isFirstFrame = true;   
        }

        public virtual void Start()
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.Start();
            }
            foreach (var gameObject in _uiObjects)
            {
                gameObject.Start();
            }
        }

        public void UpdateScene()
        {
            if (_isFirstFrame)
            {
                Start();
                _isFirstFrame = false;
            }

            foreach (var gameObject in _gameObjects)
            {
                gameObject.Update();
            }
            foreach (var gameObject in _uiObjects)
            {
                gameObject.Update();
            }

            Solver.Update();

            foreach (var gameObject in _gameObjects)
            {
                gameObject.LateUpdate();
            }
            foreach (var gameObject in _uiObjects)
            {
                gameObject.LateUpdate();
            }
        }

        public void DrawScene(SpriteBatch spriteBatch)
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.Draw(spriteBatch);
            }
        }

        public void DrawUI(SpriteBatch spriteBatch)
        {
            foreach (var uiObject in _uiObjects)
            {
                uiObject.Draw(spriteBatch);
            }
        }

        public void DrawComponents(GraphicsDevice graphics, SpriteBatch spriteBatch)
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.DrawComponents(graphics, spriteBatch);
            }
        }

        public GameObject FindWithTag(string tag)
        {
            foreach (var gameObject in _gameObjects)
            {
                if (gameObject.Tag == tag)
                    return gameObject;
            }
            return null;
        }

        public void AddGameObject(GameObject gameObject)
        {
            //Probably not the best approach
            if (gameObject as WorldObject != null)
                _gameObjects.Add(gameObject as WorldObject);

            else if (gameObject as UIObject != null)
                _uiObjects.Add(gameObject as UIObject);

            else
                throw new System.Exception("Unknown GameObject type");
        }
        public void RemoveGameObject(GameObject gameObject)
        {
            if (gameObject as WorldObject != null)
                _gameObjects.Remove(gameObject as WorldObject);

            else if (gameObject as UIObject != null)
                _uiObjects.Remove(gameObject as UIObject);

            else
                throw new System.Exception("Unknown GameObject type");
        }

        public void AddRb(RigidBody rb)
        {
            Solver.RigidBodies.Add(rb);
        }

        public void RemoveRb(RigidBody rb)
        {
            Solver.RigidBodies.Remove(rb);
        }
    }
}