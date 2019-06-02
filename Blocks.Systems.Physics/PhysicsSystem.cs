using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Blocks.Core;
using Blocks.ECS;
using Blocks.Hierarchy;
using Blocks.Library.Components;
using Blocks.Library.Components.Physics;
using Blocks.Library.Components.Rendering;
using Blocks.Systems.Transform;
using BulletSharp;
using BulletSharp.Math;

namespace Blocks.Systems.Physics
{
    public class PhysicsSystem : System<Block, PhysicsSystemEntityInfo>
    {
        private CollisionConfiguration _collisionConfiguration;
        private Dispatcher _dispatcher;
        private DbvtBroadphase _broadPhase;
        private DynamicsWorld _world;

        private bool _needCancel = false;

        private long _previousTime = Stopwatch.GetTimestamp();

        public override void Initialize(Block rootNode)
        {
            InitializePhysics();
            base.Initialize(rootNode);
            InitializeSimulationThread();
        }

        public override void Dispose()
        {
            base.Dispose();
            _needCancel = true;
            SimulationRunning = false;
            _collisionConfiguration.Dispose();
            _dispatcher.Dispose();
            _broadPhase.Dispose();
            _world.Dispose();
        }

        protected override void SetupSystem(SystemConfig config)
        {
            config.RequiresComponent<TransformComponent>();
            config.RequiresComponent<RendererComponent>();
            config.RequiresComponent<RigidBodyComponent>();
        }

        public bool SimulationRunning { get; set; } = false;

        private Thread _simulationThread;

        private void InitializePhysics()
        {
            _collisionConfiguration = new DefaultCollisionConfiguration();
            _dispatcher = new CollisionDispatcher(_collisionConfiguration);
            _broadPhase = new DbvtBroadphase();
            _world = new DiscreteDynamicsWorld(_dispatcher, _broadPhase, null, _collisionConfiguration)
            {
                Gravity = new Vector3(0, 0, -10)
            };
        }

        private void InitializeSimulationThread()
        {
            _simulationThread = new Thread(() =>
            {
                while (!_needCancel)
                {
                    SimulationStep();
                    Thread.Sleep(1);
                }
            });
            _simulationThread.Start();
        }
        
        private void SimulationStep()
        { 
            var curTime = Stopwatch.GetTimestamp();
            var delta = 1f * (curTime - _previousTime) / Stopwatch.Frequency;
            _previousTime = curTime;

            if (!SimulationRunning)
                return;
            _world.StepSimulation(delta);
            UpdateTransforms();
        }

        private void UpdateTransforms()
        {
            foreach (var kvp in EntityInfo)
            {
                var block = kvp.Key;
                var body = kvp.Value.Body;

                var matrix = body.WorldTransform;

                block.SetWorldTransform(matrix);
            }
        }

        protected override PhysicsSystemEntityInfo GenerateEntityInfo(Block entity)
        {
            return new PhysicsSystemEntityInfo
            {
                Body = entity.BuildCollisionObject()
            };
        }

        protected override void TraverseAfterAddedAction(Block entity)
        {
            base.TraverseAfterAddedAction(entity);

            if (EntityInfo.ContainsKey(entity))
            {
                _world.AddCollisionObject(EntityInfo[entity].Body);
                _world.UpdateAabbs();
            }
        }

        protected override void TraverseBeforeRemovedAction(Block entity)
        {
            base.TraverseBeforeRemovedAction(entity);

            if (EntityInfo.ContainsKey(entity))
            {
                _world.RemoveCollisionObject(EntityInfo[entity].Body);
                _world.UpdateAabbs();
            }
        }

        protected override void EntityComponentChanged(object sender, ComponentPropertyChangedEventArgs e)
        {
            if (!(sender is Block block))
                return;
            if (SimulationRunning && e.Component.GetType() == typeof(TransformComponent))
                return;
            RemoveNode(block);
            AddNode(block);
        }
    }
}