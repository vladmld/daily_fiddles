using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Structs
{
    [Serializable]
    public struct CustomVector3
    {
        public const float kEpsilon = 1E-05F;

        //
        // Summary:
        //     X component of the vector.
        public float X { get; set; }

        //
        // Summary:
        //     Y component of the vector.
        public float Y { get; set; }

        //
        // Summary:
        //     Z component of the vector.
        public float Z { get; set; }

        public CustomVector3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public override string ToString()
        {
            return String.Format("({0},{1},{2})", this.X, this.Y, this.Z);
        }
    }
}
