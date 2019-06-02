using System;
using System.Collections.Generic;
using Blocks.Core;
using Blocks.ECS;
using Blocks.Hierarchy;
using Blocks.Library.Components;
using Blocks.Utils;
using BulletSharp.Math;

namespace Blocks.Systems.Transform
{
    public sealed class TransformSystem : System<Block, TransformSystemBlockInfo>
    {
        public Matrix GetLocalTransform(Block block) => EntityInfo[block].LocalTransform;

        public Matrix GetWorldTransform(Block block) => EntityInfo[block].WorldTransform;

        public void SetWorldTransform(Block block, Matrix matrix)
        {
            var oldMatrix = EntityInfo[block].WorldTransform;
            var delta = matrix * Matrix.Invert(oldMatrix);
            var newMatrix = delta * EntityInfo[block].LocalTransform;
            newMatrix.Decompose(out var scale, out var rotationQuaternion, out var position);

            var transform = block.GetComponent<TransformComponent>();
            
            transform.BeginChange();
            transform.Position.Value = position;
            transform.Scale.Value = scale;
            transform.Rotation.Value = rotationQuaternion.ToEulerAngles();
            transform.EndChange();
        }

        private Matrix CalculateMatrix(TransformComponent transform)
        {
            var scaling = Matrix.Scaling(
                transform.Scale.Value.X,
                transform.Scale.Value.Y,
                transform.Scale.Value.Z);
            var rotation = Matrix.RotationYawPitchRoll(
                transform.Rotation.Value.Y,
                transform.Rotation.Value.X,
                transform.Rotation.Value.Z);
            var translation = Matrix.Translation(transform.Position.Value);

            return scaling * rotation * translation;
        }

        protected override void SetupSystem(SystemConfig config)
        {
            config.RequiresComponent<TransformComponent>();
        }

        protected override TransformSystemBlockInfo GenerateEntityInfo(Block entity)
        {
            var transform = entity.GetComponent<TransformComponent>();

            var matrix = CalculateMatrix(transform);
            return new TransformSystemBlockInfo
            {
                LocalTransform = matrix,
                WorldTransform = entity.Parent == null ? matrix : EntityInfo[entity.Parent].WorldTransform * matrix
            };
        }

        protected override void EntityComponentChanged(object sender, ComponentPropertyChangedEventArgs e)
        {
            if (!(sender is Block block))
                return;

            if (e.Component is TransformComponent transform)
            {
                var oldLocalTransform = EntityInfo[block].LocalTransform;
                var newLocalTransform = CalculateMatrix(transform);
                var delta = newLocalTransform * Matrix.Invert(oldLocalTransform);

                block.Traverse(b => EntityInfo[b].WorldTransform = delta * EntityInfo[b].WorldTransform);

                EntityInfo[block].LocalTransform = delta * EntityInfo[block].LocalTransform;
            }
        }
    }
}
