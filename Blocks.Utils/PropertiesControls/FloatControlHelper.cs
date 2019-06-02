using System.Windows;
using System.Windows.Controls;
using Blocks.Core;
using Blocks.ECS;
using BulletSharp.Math;

namespace Blocks.Utils.PropertiesControls
{
    public static class FloatControlHelper
    {
        public static FrameworkElement BuildControl(this Property<float> prop)
        {
            var textBox = new TextBox { Text = prop.Value.ToString(), Width = 150 };

            void Handle(object s, TextChangedEventArgs e)
            {
                var success = textBox.Text.ToFloat(out var val);
                if (!success)
                    return;
                prop.Value = val;
            };

            textBox.TextChanged += Handle;

            prop.ValueChanged += (s, e) =>
            {
                if (!textBox.Text.ToFloat(out var val) || val != prop.Value)
                    textBox.Text = prop.Value.ToString();
            };

            return textBox;
        }
    }
}