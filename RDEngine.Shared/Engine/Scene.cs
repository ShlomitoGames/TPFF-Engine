using RDEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RDEngine.Engine.Physics;
using System.Collections.Generic;
using RDEngine.Engine.UI;
using System.Diagnostics;

namespace RDEngine.Engine
{
    public class Scene
    {
        private List<WorldObject> _worldObjects;
        private List<UIObject> _uiObjects;

        private List<WorldObject> _newWorldObjects;
        private List<UIObject> _newUIObjects;

        private byte _unitSize;
        public byte UnitSize { get { return _unitSize; } }

        public Vector2 CameraPos;
        public Vector2 PixelCameraPos
        {
            get
            {
                return Vector2.Floor(CameraPos / RDEGame.ScaleFactor);
            }
        }
        public Vector2 CameraOrigin
        {
            get
            {
                return CameraPos + RDEGame.UpscaledScrSize / 2f;
            }
            set
            {
                CameraPos = value - RDEGame.UpscaledScrSize / 2f;
            }
        }
        public Vector2 PixelCameraOrigin
        {
            get
            {
                return CameraPos / RDEGame.ScaleFactor + RDEGame.ScreenSize / 2f;
            }
            set
            {
                CameraPos = (value - RDEGame.ScreenSize / 2f) * RDEGame.ScaleFactor;
            }
        }

        public Color CameraColor;

        public PhysicsSolver Solver;

        private bool _isFirstFrame;

        public Scene(Color camColor, byte unitSize = 16)
        {
            _unitSize = unitSize;
            CameraColor = camColor;
            CameraOrigin = Vector2.Zero;
        }

        public virtual void Initialize()
        {
            _isFirstFrame = true;
            Solver = new PhysicsSolver();

            _worldObjects = new List<WorldObject>();
            _uiObjects = new List<UIObject>();

            _newWorldObjects = new List<WorldObject>();
            _newUIObjects = new List<UIObject>();
        }

        public virtual void Start()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void OnDelete()
        {

        }

        public void UpdateSceneElements()
        {
            _worldObjects.AddRange(_newWorldObjects);
            _uiObjects.AddRange(_newUIObjects);
            WorldObject[] woToAdd = _newWorldObjects.ToArray();
            UIObject[] uoToAdd = _newUIObjects.ToArray();

            _newWorldObjects = new List<WorldObject>();
            _newUIObjects = new List<UIObject>();

            foreach (var gameObject in woToAdd)
            {
                gameObject.Scene = this;
                gameObject.Start();
            }
            foreach (var gameObject in uoToAdd)
            {
                gameObject.Scene = this;
                gameObject.Start();
            }

            if (_isFirstFrame)
            {
                Start();
                _isFirstFrame = false;
            }

            Update();
            foreach (var gameObject in _worldObjects)
            {
                gameObject.Update();
            }
            foreach (var gameObject in _uiObjects)
            {
                gameObject.Update();
            }

            Solver.Update();

            foreach (var gameObject in _worldObjects)
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
            foreach (var gameObject in _worldObjects)
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
            foreach (var gameObject in _worldObjects)
            {
                gameObject.DrawComponents(graphics, spriteBatch);
            }
        }

        public GameObject FindWithTag(string tag)
        {
            foreach (var gameObject in _worldObjects)
            {
                if (gameObject.Tag == tag)
                    return gameObject;
            }
            foreach (var gameObject in _uiObjects)
            {
                if (gameObject.Tag == tag)
                    return gameObject;
            }
            return null;
        }

        public void AddGameObjects(GameObject[] gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                AddGameObject(gameObject);
            }
        }
        public void AddGameObject(GameObject gameObject)
        {
            //Probably not the best approach
            if (gameObject as WorldObject != null)
                _newWorldObjects.Add(gameObject as WorldObject);

            else if (gameObject as UIObject != null)
                _newUIObjects.Add(gameObject as UIObject);

            else
                throw new System.Exception("Unknown GameObject type");
        }
        public void RemoveGameObject(GameObject gameObject)
        {
            if (gameObject as WorldObject != null)
                _worldObjects.Remove(gameObject as WorldObject);

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