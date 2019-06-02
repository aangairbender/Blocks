using Blocks.Core;
using Blocks.ECS;
using Blocks.Library.Components;
using Blocks.Library.Components.Physics;
using Blocks.Library.Components.Rendering;
using Blocks.Systems.Transform;
using BulletSharp;
using BulletSharp.Math;

namespace Blocks.Systems.Physics
{
    public static class PhysicsHelper
    {
        public static RigidBody BuildCollisionObject(this Block block)
        {
            var transform = block.GetComponent<TransformComponent>();
            var renderer = block.GetComponent<RendererComponent>();
            var rigidBody = block.GetComponent<RigidBodyComponent>();

            if (transform == null || renderer == null || rigidBody == null)
                return null;

            var transformMatrix = SystemManager<Block>.GetSystem<TransformSystem>().GetWorldTransform(block);

            var mass = rigidBody.IsKinematic.Value ? 0 : rigidBody.Mass.Value;
            var colliderShape = renderer.BuildCollisionShape();
            colliderShape.LocalScaling = transform.Scale.Value;

            var localInertia = colliderShape.CalculateLocalInertia(mass);

            var info = new RigidBodyConstructionInfo(
                mass,
                new DefaultMotionState(transformMatrix),
                colliderShape,
                localInertia)
            {
                Friction = rigidBody.Friction.Value,
                Restitution = rigidBody.Restitution.Value
            };

            var physBody = new RigidBody(info);
            if (rigidBody.IsKinematic.Value)
            {
                physBody.CollisionFlags |= CollisionFlags.KinematicObject;
                physBody.ActivationState = ActivationState.DisableDeactivation;
            }
            info.Dispose();

            return physBody;
        }
    }
}