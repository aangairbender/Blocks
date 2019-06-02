using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Blocks.Core;
using Blocks.UI.Windows;
using BulletSharp;

namespace Blocks
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var w = new ProjectWindow();

            w.DataContext = new ProjectViewModel(new Project("Test project", "D:/Test project"));
            w.Show();
        }
    }
}
