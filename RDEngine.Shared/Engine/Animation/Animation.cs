using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RDEngine.Engine.Animation
{
    public class Animation
    {
        internal Animator _animator;

        public AnimLayer[] Layers;

        public string Name;

        public enum VarTypes
        {
            Int,
            Float,
            Bool,
            Texture2D
        }

        public bool Loop;

        public bool Enabled;

        private float _time;
        private int _totalDuration;

        public Animation(bool loop, AnimLayer[] layers)
        {
            Loop = loop;
            Reset();

            Layers = layers;
            _totalDuration = 0;
            foreach (var layer in Layers)
            {
                if (layer.TotalDuration > _totalDuration)
                    _totalDuration = layer.TotalDuration;
            }
        }

        internal void SetAnimator(Animator animator)
        {
            _animator = animator;
        }

        internal void Reset()
        {
            _time = 0;
            Enabled = true;

            if (Layers != null)
            {
                foreach (var layer in Layers)
                {
                    layer.Time = 0;
                    layer.CurrKeyFrame = 0;
                }
            }
        }

        internal void Start()
        {
            foreach (var layer in Layers)
            {
                if (layer.Type == VarTypes.Int)
                    _animator.Ints[layer.VarIndex] = layer.GetValue<int>();
                else if (layer.Type == VarTypes.Float)
                    _animator.Floats[layer.VarIndex] = layer.GetValue<float>();
                else if (layer.Type == VarTypes.Bool)
                    _animator.Bools[layer.VarIndex] = layer.GetValue<bool>();
                else
                    _animator.Textures[layer.VarIndex] = layer.GetValue<Texture2D>();
            }
        }

        internal void StepAnimation()
        {
            if (!Enabled || _animator == null) return;
            
            foreach (var layer in Layers)
            {
                //Things that happen on the Rising Edge
                if (layer.Time == 0)
                {
                    if (layer.Type == VarTypes.Bool)
                        _animator.Bools[layer.VarIndex] = layer.GetValue<bool>();
                    else if (layer.Type == VarTypes.Texture2D)
                        _animator.Textures[layer.VarIndex] = layer.GetValue<Texture2D>();
                }
                //On all but the last keyframe
                if (layer.CurrKeyFrame < layer.Durations.Length - 1)
                {
                    //Things that happen Continuously
                    if (layer.Time < layer.Durations[layer.CurrKeyFrame])
                    {
                        layer.Time += Time.DeltaTime * 1000f;

                        if (layer.Type == VarTypes.Int)
                        {
                            int val1 = layer.GetValue<int>();
                            int val2 = layer.GetValue<int>(layer.CurrKeyFrame + 1);
                            int min = val2 > val1 ? val1 : val2;
                            int max = val2 < val1 ? val1 : val2;

                            _animator.Ints[layer.VarIndex] = MathHelper.Clamp((int)MathHelper.Lerp(val1, val2, (float)layer.Time / (float)layer.CurrKeyFrameDuration), min, max);
                        }
                        else if (layer.Type == VarTypes.Float)
                        {
                            float val1 = layer.GetValue<float>();
                            float val2 = layer.GetValue<float>(layer.CurrKeyFrame + 1);
                            float min = val2 > val1 ? val1 : val2;
                            float max = val2 < val1 ? val1 : val2;

                            _animator.Floats[layer.VarIndex] = MathHelper.Clamp(MathHelper.Lerp(val1, val2, (float)layer.Time / (float)layer.CurrKeyFrameDuration), min, max);
                        }
                    }
                    //If it's done with that keyframe,
                    //and the animation cycle hasn't finished
                    else if (_time < _totalDuration)
                    {
                        layer.CurrKeyFrame++;
                        layer.Time = 0;
                    }
                }
            }
            //If the longest layer has finished
            if (_time >= _totalDuration)
            {
                _time = 0;
                foreach (var layer in Layers)
                {
                    layer.Time = 0;
                    layer.CurrKeyFrame = 0;
                }
                if (!Loop)
                    Enabled = false;
            }
            _time += Time.DeltaTime * 1000f;
        }
    }
}
