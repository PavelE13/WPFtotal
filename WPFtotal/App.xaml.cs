using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPFtotal.ViewModels;

namespace WPFtotal
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void OnStartup(object sender, StartupEventArgs e) // Create the ViewModel and expose it using the View's DataContext
        {
            Views.MainWindow view = new Views.MainWindow();
            view.DataContext = new MainWindowViewModel();
            view.Show();
        }
    }
}
