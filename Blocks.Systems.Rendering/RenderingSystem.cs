using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Blocks.Core;
using Blocks.ECS;
using Blocks.Hierarchy;
using Blocks.Library.Components;
using Blocks.Library.Components.Rendering;
using Blocks.Systems.Transform;
using Blocks.Utils;
using HelixToolkit.Wpf;

namespace Blocks.Systems.Rendering
{
    public sealed class RenderingSystem : System<Block, RenderingSystemEntityInfo>
    {
        public Model3D RootModel3D => EntityInfo[RootNode].Group;

        private Model3DGroup CreateGroup(Block block)
        {
            return new Model3DGroup
            {
                Transform = new MatrixTransform3D(block.GetLocalTransform().ToMatrix3D())
            };
        }

        private GeometryModel3D CreateModel(Block block)
        {
            var renderer = block.GetComponent<RendererComponent>();

            return renderer?.BuildModel();
        }

        protected override void SetupSystem(SystemConfig config)
        {
            config.RequiresComponent<TransformComponent>();
        }

        protected override RenderingSystemEntityInfo GenerateEntityInfo(Block entity)
        {
            var group = CreateGroup(entity);
            var model = CreateModel(entity);

            if (model != null)
            {
                model.Freeze();
                group.Children.Add(model);
            }
           
            return new RenderingSystemEntityInfo
            {
                Group = group,
                Model = model
            };
        }

        protected override void TraverseAfterAddedAction(Block entity)
        {
            base.TraverseAfterAddedAction(entity);

            if (entity.Parent == null)
                return;

            EntityInfo[entity.Parent].Group.Children.Add(EntityInfo[entity].Group);
        }

        protected override void TraverseBeforeRemovedAction(Block entity)
        {
            base.TraverseBeforeRemovedAction(entity);

            if (entity.Parent == null)
                return;

            EntityInfo[entity.Parent].Group.Children.Remove(EntityInfo[entity].Group);
        }

        protected override void EntityComponentChanged(object sender, ComponentPropertyChangedEventArgs e)
        {
            if (!(sender is Block block))
                return;

            if (e.Component.GetType() == typeof(TransformComponent))
            {
                var group = EntityInfo[block].Group;
                group.Dispatcher.Invoke(() =>
                    group.Transform = new MatrixTransform3D(block.GetLocalTransform().ToMatrix3D()));
                return;
            }

            RemoveNode(block);
            AddNode(block);
        }
    }
}