using System.Windows;
using Blocks.Core;
using BulletSharp.Math;
using System.Windows.Controls;
using Blocks.ECS;

namespace Blocks.Utils.PropertiesControls
{
    public static class Vector3ControlHelper
    {
        public static FrameworkElement BuildControl(this Property<Vector3> prop)
        {
            var stack = new StackPanel { Orientation = Orientation.Horizontal };

            stack.Children.Add(new TextBlock { Text = "X", Margin = new Thickness(10, 0, 10, 0) });
            var xTextBox = new TextBox { Text = prop.Value.X.ToString(), Width = 40 };
            stack.Children.Add(xTextBox);

            stack.Children.Add(new TextBlock { Text = "Y", Margin = new Thickness(10, 0, 10, 0) });
            var yTextBox = new TextBox { Text = prop.Value.Y.ToString(), Width = 40 };
            stack.Children.Add(yTextBox);

            stack.Children.Add(new TextBlock { Text = "Z", Margin = new Thickness(10, 0, 10, 0) });
            var zTextBox = new TextBox { Text = prop.Value.Z.ToString(), Width = 40 };
            stack.Children.Add(zTextBox);

            void Handle(object s, TextChangedEventArgs e)
            {
                var success = true;
                success &= xTextBox.Text.ToFloat(out var x);
                success &= yTextBox.Text.ToFloat(out var y);
                success &= zTextBox.Text.ToFloat(out var z);
                if (!success)
                    return;
                prop.Value = new Vector3(x, y, z);
            };

            xTextBox.TextChanged += Handle;
            yTextBox.TextChanged += Handle;
            zTextBox.TextChanged += Handle;

            prop.ValueChanged += (s, e) =>
            {
                var px = prop.Value.X;
                var py = prop.Value.Y;
                var pz = prop.Value.Z;
                xTextBox.Dispatcher.Invoke(() =>
                {
                    if (!xTextBox.Text.ToFloat(out var x) || x != px)
                        xTextBox.Text = px.ToString();
                    if (!yTextBox.Text.ToFloat(out var y) || y != py)
                        yTextBox.Text = py.ToString();
                    if (!zTextBox.Text.ToFloat(out var z) || z != pz)
                        zTextBox.Text = pz.ToString();
                });
            };

            return stack;
        }
    }
}