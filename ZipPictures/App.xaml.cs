using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace ZipPictures
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string zipfolder = "";

            if (e.Args.Length != 0)
            {
                string argFolder = e.Args[0];
                if (Directory.Exists(argFolder))
                {
                    zipfolder = Path.GetFullPath(argFolder);
                }
            }
            MainWindow mainWindow = new MainWindow();
            mainWindow.StartupFolder = zipfolder;
            MainWindow.Show();
        }
    }
}
