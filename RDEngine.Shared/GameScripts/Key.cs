using Microsoft.Xna.Framework.Audio;
using RDEngine.Engine;
using RDEngine.Engine.Physics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDEngine.GameScripts
{
    public class Key : GComponent, ICollideable
    {
        private Door _door;
        private SoundEffect _keySound;
        private bool _open;
        private float _time;

        private float _duration;

        public Key(Door door)
        {
            _door = door;
            _keySound = ContentStorer.SFX["Key"];
            _open = false;
            _duration = 1f;
        }

        public override void Update()
        {
            if (_open)
            {
                _time += Time.DeltaTime;

                if (_time >= _duration)
                {
                    _door.Open();
                    Parent.Enabled = false;
                }
            }
        }

        void ICollideable.OnTriggerEnter(RigidBody intrRb)
        {
            if (intrRb.Parent.Tag == "Player" && !_open)
            {
                //ooohhhohoho ohh ho boy
                Parent.Texture = null;
                _keySound.Play();
                _open = true;
            }
        }
    }
}
