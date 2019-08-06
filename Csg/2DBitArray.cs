using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Csg
{
    class BitArray2D {
        readonly int Width;
        readonly int Height;

        readonly private BitArray array;


        public BitArray2D(int width, int height) {
            this.Width = width;
            this.Height = height;
            array = new BitArray(width * height);
        }

        public bool this[int x, int y] {
            get {
                return array[(x * Width) + y];
			}
            set {
                array[(x * Width) + y] = value;
            }
        }
    }
}
