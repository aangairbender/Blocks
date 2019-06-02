using System.Windows;
using System.Windows.Controls;
using Blocks.Core;
using Blocks.ECS;

namespace Blocks.Utils.PropertiesControls
{
    public static class IntControlHelper
    {
        public static FrameworkElement BuildControl(this Property<int> prop)
        {
            var textBox = new TextBox { Text = prop.Value.ToString(), Width = 150 };

            void Handle(object s, TextChangedEventArgs e)
            {
                var success = textBox.Text.ToInt(out var val);
                if (!success)
                    return;
                prop.Value = val;
            };

            textBox.TextChanged += Handle;

            prop.ValueChanged += (s, e) =>
            {
                if (!textBox.Text.ToInt(out var val) || val != prop.Value)
                    textBox.Text = prop.Value.ToString();
            };

            return textBox;
        }
    }
}