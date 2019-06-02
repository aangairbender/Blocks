using BulletSharp.Math;

namespace Blocks.Systems.Transform
{
    public class TransformSystemBlockInfo
    {
        public Matrix LocalTransform { get; set; }
        public Matrix WorldTransform { get; set; }
    }
}