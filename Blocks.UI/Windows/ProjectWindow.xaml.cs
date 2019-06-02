using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Windows.Threading;
using Blocks.Core;
using Blocks.ECS;
using Blocks.Library.Components;
using Blocks.Library.Components.Physics;
using Blocks.Library.Components.Rendering;
using Blocks.Systems.Physics;
using Blocks.Systems.Rendering;
using Blocks.Systems.Transform;
using BulletSharp.Math;
using HelixToolkit.Wpf;

namespace Blocks.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для ProjectWindow.xaml
    /// </summary>
    public partial class ProjectWindow : Window
    {
        public ProjectWindow()
        {
            InitializeComponent();
        }

        private void ProjectWindow_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var rootBlock = ((ProjectViewModel) DataContext).Workspace.RootBlock;
            rootBlock.AddComponent<TransformComponent>();

            var sphere = new Block("Sphere");
            sphere.AddComponent<TransformComponent>();
            sphere.AddComponent<SphereRendererComponent>();
            sphere.AddComponent<RigidBodyComponent>();


            var box = new Block("Box");
            box.AddComponent<TransformComponent>();
            box.AddComponent<BoxRendererComponent>();
            box.AddComponent<RigidBodyComponent>();

            box.GetComponent<TransformComponent>().Position.Value = new Vector3(0, 0, -10);
            box.GetComponent<RigidBodyComponent>().IsKinematic.Value = true;

            rootBlock.AddChild(sphere);
            rootBlock.AddChild(box);

            SystemManager<Block>.RegisterSystem(new TransformSystem());
            SystemManager<Block>.RegisterSystem(new RenderingSystem());
            SystemManager<Block>.RegisterSystem(new PhysicsSystem());

            SystemManager<Block>.InitializeSystems(rootBlock);

            SceneRoot.Children.Clear();
            SceneRoot.Children.Add(new ModelVisual3D
            {
                Content = SystemManager<Block>.GetSystem<RenderingSystem>().RootModel3D
            });
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            // Physics
            SystemManager<Block>.GetSystem<PhysicsSystem>().SimulationRunning = true;
        }

        private void ButtonBase_OnClick2(object sender, RoutedEventArgs e)
        {
            SystemManager<Block>.GetSystem<PhysicsSystem>().SimulationRunning = false;
        }

        private void ProjectWindow_OnClosing(object sender, CancelEventArgs e)
        {
            SystemManager<Block>.Dispose();
        }
    }
}
