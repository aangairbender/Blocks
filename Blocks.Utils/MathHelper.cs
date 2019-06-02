using System;
using System.Windows.Media.Media3D;
using BulletSharp.Math;
using Quaternion = BulletSharp.Math.Quaternion;

namespace Blocks.Utils
{
    public static class MathHelper
    {
        public static Vector3 ToEulerAngles(this Quaternion q)
        {
            var sqw = q.W * q.W;
            var sqx = q.X * q.X;
            var sqy = q.Y * q.Y;
            var sqz = q.Z * q.Z;

            return new Vector3(
                (float) Math.Atan2(2f * (q.Y * q.Z + q.X * q.W), -sqx - sqy + sqz + sqw),
                (float) -Math.Asin(2f * (q.X * q.Z - q.Y * q.W)),
                (float) Math.Atan2(2f * (q.X * q.Y + q.Z * q.W), sqx - sqy - sqz + sqw));
        }
    }
}