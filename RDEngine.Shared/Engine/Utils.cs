using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDEngine.Engine
{
    public static class Utils
    {
        public static void Swap<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }
        public class Wrapper<T> where T : struct
        {
            public T Value { get; set; }
        }
    }
}
