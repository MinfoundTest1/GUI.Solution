using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace RxWpfTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MemberSearchViewModel vm = new MemberSearchViewModel();
            MainWindow mw = new MainWindow(vm);
            mw.Show();
        }
    }
}
