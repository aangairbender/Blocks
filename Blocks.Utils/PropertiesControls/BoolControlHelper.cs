using System.Windows;
using System.Windows.Controls;
using Blocks.Core;
using Blocks.ECS;

namespace Blocks.Utils.PropertiesControls
{
    public static class BoolControlHelper
    {
        public static FrameworkElement BuildControl(this Property<bool> prop)
        {
            var checkBox = new CheckBox{IsThreeState = false, IsChecked = prop.Value};

            void Handle(object s, RoutedEventArgs e)
            {
                if (!checkBox.IsChecked.HasValue)
                    return;
                prop.Value = checkBox.IsChecked.Value;
            };

            checkBox.Checked += Handle;
            checkBox.Unchecked += Handle;

            prop.ValueChanged += (s, e) =>
            {
                if (checkBox.IsChecked != prop.Value)
                    checkBox.IsChecked = prop.Value;
            };

            return checkBox;
        }
    }
}