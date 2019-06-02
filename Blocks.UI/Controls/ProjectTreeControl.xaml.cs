using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Blocks.Core;
using Blocks.Library.Components;
using Blocks.Library.Components.Physics;
using Blocks.Library.Components.Rendering;
using Blocks.UI.Windows;
using PropertyTools.Wpf;

namespace Blocks.UI.Controls
{
    /// <summary>
    /// Логика взаимодействия для ProjectTreeControl.xaml
    /// </summary>
    public partial class ProjectTreeControl : UserControl
    {
        public ProjectTreeControl()
        {
            InitializeComponent();
        }

        private void SelectedBlockChangedHandler(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;
            ((ProjectViewModel)DataContext).SelectedBlock = (e.AddedItems[0] as NodeViewModel).Node;
        }

        private void TreeViewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F2:
                    var cvm = ((TreeListBox) sender).SelectedItem as NodeViewModel;
                    if (cvm != null)
                        cvm.IsEditing = true;
                    break;
                case Key.Delete:
                    DeleteSelected();
                    break;
            }

        }

        private void DeleteSelected()
        {
            var idx = ProjectStructure.SelectedIndex;
            var td = new List<NodeViewModel>();
            foreach (NodeViewModel s in ProjectStructure.SelectedItems)
            {
                td.Add(s);
            }

            foreach (var s in td)
            {
                if (s.Parent != null)
                {
                    s.Parent.Children.Remove(s);
                    s.Parent.Node.RemoveChild(s.Node);
                }
            }

            ProjectStructure.SelectedIndex = idx < ProjectStructure.Items.Count ? idx : idx - 1;
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            DeleteSelected();
        }

        private void MenuItem_OnClick2(object sender, RoutedEventArgs e)
        {
            var block = new Block("New Block");
            block.AddComponent<TransformComponent>();
            (ProjectStructure.SelectedItem as NodeViewModel).AddChild(block, true);
        }

        private void AddSphereRendererHandler(object sender, RoutedEventArgs e)
        {
            var block = (ProjectStructure.SelectedItem as NodeViewModel).Node;
            if (block.GetComponent<RendererComponent>() != null)
                return;
            block.AddComponent<SphereRendererComponent>();
        }

        private void AddBoxRendererHandler(object sender, RoutedEventArgs e)
        {
            var block = (ProjectStructure.SelectedItem as NodeViewModel).Node;
            if (block.GetComponent<RendererComponent>() != null)
                return;
            block.AddComponent<BoxRendererComponent>();
        }

        private void AddRigidbodyComponent(object sender, RoutedEventArgs e)
        {
            var block = (ProjectStructure.SelectedItem as NodeViewModel).Node;
            if (block.GetComponent<RigidBodyComponent>() != null)
                return;
            block.AddComponent<RigidBodyComponent>();
        }

        private void MenuItem_OnClick3(object sender, RoutedEventArgs e)
        {
            var block = (ProjectStructure.SelectedItem as NodeViewModel).Node.Clone();
            (ProjectStructure.SelectedItem as NodeViewModel).Parent.AddChild(block, true);
        }
    }
}
