using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace RDEngine.Engine.Animation
{
    public class Animator : GComponent
    {
        private Animation _currAnim;
        private string _currAnimName;
        private Dictionary<string, Animation> _animations;

        internal int[] Ints;
        internal float[] Floats;
        internal bool[] Bools;
        internal Texture2D[] Textures;

        public Animator() : this(new Dictionary<string, Animation>(), null)
        {

        }
        public Animator(Dictionary<string, Animation> anims, string startState, int[] ints = null, float[] floats = null, bool[] bools = null, Texture2D[] textures = null)
        {
            Ints = ints;
            Floats = floats;
            Bools = bools;
            Textures = textures;

            SetAnimations(anims, startState);
        }

        public override void Update()
        {
            _currAnim.StepAnimation();
            base.Update();
        }

        public void SetAnimations(Dictionary<string, Animation> anims, string startAnim)
        {
            _animations = anims;
            if (anims.Count > 0)
            {
                foreach (var anim in anims)
                {
                    anim.Value.SetAnimator(this);
                    anim.Value.Name = anim.Key;
                }
                SetAnimation(startAnim);
            }
        }

        public string GetAnimName()
        {
            return _currAnimName;
        }
        public Animation GetAnimation()
        {
            return _currAnim;
        }
        public void SetAnimation(string anim)
        {
            _currAnim = _animations[anim];
            _currAnim.Reset();
            _currAnimName = anim;
        }
    }
}
