using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RDEngine.Engine.Animation
{
    public class AnimLayer
    {
        //The first int is the duration of the keyframe
        private int[] _ints;
        private float[] _floats;
        private bool[] _bools;
        private Texture2D[] _textures;
        private SoundEffect[] _sounds;

        private int[] _durations;
        internal int[] Durations
        {
            private set
            {
                _durations = value;
                foreach (var i in value)
                {
                    TotalDuration += i;
                }
            }
            get
            {
                return _durations;
            }
        }
        internal int TotalDuration;
        internal int CurrKeyFrameDuration
        {
            get
            {
                return Durations[CurrKeyFrame];
            }
        }

        public Animation.VarTypes Type;
        public int VarIndex; //The index of the variable in the corresponding Animator array

        internal float Time;
        internal int CurrKeyFrame; //The index in the array of the current keyframe

        private AnimLayer(int index)
        {
            VarIndex = index;
            Time = 0;
            CurrKeyFrame = 0;
            TotalDuration = 0;
        }
        public AnimLayer(Tuple<int, int>[] value, int index) : this(index)
        {
            _ints = Array.ConvertAll<Tuple<int, int>, int>(value, x => x.Item2);
            _floats = null;
            _bools = null;
            _textures = null;
            _sounds = null;
            Type = Animation.VarTypes.Int;

            Durations = Array.ConvertAll<Tuple<int, int>, int>(value, x => x.Item1);
        }
        public AnimLayer(Tuple<int, float>[] value, int index) : this(index)
        {
            _ints = null;
            _floats = Array.ConvertAll<Tuple<int, float>, float>(value, x => x.Item2);
            _bools = null;
            _textures = null;
            _sounds = null;
            Type = Animation.VarTypes.Float;

            Durations = Array.ConvertAll<Tuple<int, float>, int>(value, x => x.Item1);
        }
        public AnimLayer(Tuple<int, bool>[] value, int index) : this(index)
        {
            _ints = null;
            _floats = null;
            _bools = Array.ConvertAll<Tuple<int, bool>, bool>(value, x => x.Item2);
            _textures = null;
            _sounds = null;
            Type = Animation.VarTypes.Bool;

            Durations = Array.ConvertAll<Tuple<int, bool>, int>(value, x => x.Item1);
        }
        public AnimLayer(Tuple<int, Texture2D>[] value, int index) : this(index)
        {
            _ints = null;
            _floats = null;
            _bools = null;
            _textures = Array.ConvertAll<Tuple<int, Texture2D>, Texture2D>(value, x => x.Item2);
            _sounds = null;
            Type = Animation.VarTypes.Texture2D;

            Durations = Array.ConvertAll<Tuple<int, Texture2D>, int>(value, x => x.Item1);
        }
        public AnimLayer(Tuple<int, SoundEffect>[] value, int index) : this(index)
        {
            _ints = null;
            _floats = null;
            _bools = null;
            _textures = null;
            _sounds = Array.ConvertAll<Tuple<int, SoundEffect>, SoundEffect>(value, x => x.Item2);
            Type = Animation.VarTypes.Sound;

            Durations = Array.ConvertAll<Tuple<int, SoundEffect>, int>(value, x => x.Item1);
        }
        public T GetValue<T>()
        {
            return GetValue<T>(CurrKeyFrame);
        }
        public T GetValue<T>(int index)
        {
            if (Type == Animation.VarTypes.Int)
                return (T)(object)_ints[index];
            else if (Type == Animation.VarTypes.Float)
                return (T)(object)_floats[index];
            else if (Type == Animation.VarTypes.Bool)
                return (T)(object)_bools[index];
            else if (Type == Animation.VarTypes.Texture2D)
                return (T)(object)_textures[index];
            else
                return (T)(object)_sounds[index];
        }
        public T[] GetArray<T>()
        {
            if (Type == Animation.VarTypes.Int)
                return (T[])(object)_ints;
            else if (Type == Animation.VarTypes.Float)
                return (T[])(object)_floats;
            else if (Type == Animation.VarTypes.Bool)
                return (T[])(object)_bools;
            else if (Type == Animation.VarTypes.Texture2D)
                return (T[])(object)_textures;
            else
                return (T[])(object)_sounds;
        }
    }
}
