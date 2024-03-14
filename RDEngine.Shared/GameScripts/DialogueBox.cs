using Microsoft.Xna.Framework;
using RDEngine.Engine;
using RDEngine.Engine.Animation;
using RDEngine.Engine.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RDEngine.GameScripts
{
    public class DialogueBox : GComponent
    {
        private Animator _anim;
        private UIObject _parent;

        public override void Start()
        {
            _anim = Parent.GetComponent<Animator>();
            _parent = Parent as UIObject;
        }

        public override void Update()
        {
            Parent.Color = new Color(1, 1, 1, 1) * _anim.Ints[0];
            _parent.CCPosition = new Vector2(_parent.CCPosition.X, _anim.Ints[1]);

            if (_anim.GetAnimName() == "fade" && _anim.GetAnimation().Enabled == false)
                _anim.SetAnimation("off");
        }
    }
}
