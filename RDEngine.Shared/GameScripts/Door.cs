using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using RDEngine.Engine;
using RDEngine.Engine.Physics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RDEngine.GameScripts
{
    public class Door : GComponent
    {
        private SoundEffect _openSound;
        private Texture2D _openTexture;
        private RigidBody _rb;

        private int _minOpenCount, _openCount;

        private bool _open;

        public Door(int minOpenCount = 0)
        {
            _minOpenCount = minOpenCount;
            _openCount = 0;
            _open = false;
        }

        public override void Start()
        {
            Parent.Texture = ContentStorer.Textures["Door"];
            _openSound = ContentStorer.SFX["Door"];
            _openTexture = ContentStorer.Textures["DoorOpen"];
            _rb = Parent.GetComponent<RigidBody>();
        }

        public void Open()
        {
            _openCount++;
            if (_openCount < _minOpenCount || _open)
                return;

            _rb.Enabled = false;
            _openSound.Play();
            Parent.Texture = _openTexture;
            _open = true;
        }
    }
}
