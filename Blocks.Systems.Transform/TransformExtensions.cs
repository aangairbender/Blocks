using Blocks.Core;
using Blocks.ECS;
using Blocks.Library.Components;
using BulletSharp.Math;

namespace Blocks.Systems.Transform
{
    public static class TransformExtensions
    {
        public static Matrix GetLocalTransform(this Block block)
        {
            var transform = block.GetComponent<TransformComponent>();
            if (transform == null)
                return Matrix.Identity;

            return SystemManager<Block>.GetSystem<TransformSystem>().GetLocalTransform(block);
        }

        public static Matrix GetWorldTransform(this Block block)
        {
            var transform = block.GetComponent<TransformComponent>();
            if (transform == null)
                return Matrix.Identity;

            return SystemManager<Block>.GetSystem<TransformSystem>().GetWorldTransform(block);
        }

        public static void SetWorldTransform(this Block block, Matrix matrix)
        {
            var transform = block.GetComponent<TransformComponent>();
            if (transform == null)
                return;

            SystemManager<Block>.GetSystem<TransformSystem>().SetWorldTransform(block, matrix);
        }
    }
}