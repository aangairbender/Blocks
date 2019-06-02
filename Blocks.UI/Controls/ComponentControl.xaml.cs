using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Blocks.Core;
using Blocks.ECS;
using Blocks.Utils.PropertiesControls;
using BulletSharp.Math;

namespace Blocks.UI.Controls
{
    /// <summary>
    /// Логика взаимодействия для ComponentControl.xaml
    /// </summary>
    public partial class ComponentControl : UserControl
    {
        public ComponentControl()
        {
            InitializeComponent();
        }

        private void InitializeProperties()
        {
            PropertiesList.Items.Clear();
            foreach (var property in ((ComponentBase)DataContext).ComponentProperties)
            {
                var stackPanel = new StackPanel {Orientation = Orientation.Vertical};
                stackPanel.Children.Add(new TextBlock {Text = property.Name, Margin = new Thickness(0,0,10,0)});
                if (property is Property<Vector3> vectorProperty)
                {
                    stackPanel.Children.Add(vectorProperty.BuildControl());
                } else if (property is Property<float> floatProperty)
                {
                    stackPanel.Children.Add(floatProperty.BuildControl());
                }
                else if (property is Property<int> intProperty)
                {
                    stackPanel.Children.Add(intProperty.BuildControl());
                } else if (property is Property<bool> boolProperty)
                {
                    stackPanel.Children.Add(boolProperty.BuildControl());
                }

                PropertiesList.Items.Add(stackPanel);
            }
        }

        private void ComponentControl_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            InitializeProperties();
        }
    }
}
