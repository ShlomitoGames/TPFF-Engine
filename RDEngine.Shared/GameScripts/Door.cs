using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using RDEngine.Engine;
using RDEngine.Engine.Physics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDEngine.GameScripts
{
    public class Door : GComponent
    {
        private SoundEffect _openSound;
        private Texture2D _openTexture;
        private RigidBody _rb;

        public override void Start()
        {
            Parent.Texture = ContentStorer.Textures["Door"];
            _openSound = ContentStorer.SFX["Door"];
            _openTexture = ContentStorer.Textures["DoorOpen"];
            _rb = Parent.GetComponent<RigidBody>();
        }

        public void Open()
        {
            _rb.Enabled = false;
            _openSound.Play();
            Parent.Texture = _openTexture;
        }
    }
}
