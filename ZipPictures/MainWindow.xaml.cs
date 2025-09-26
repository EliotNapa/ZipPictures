using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

namespace ZipPictures
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        BackgroundWorker _backgroundWorker;
        string[] _dirs;

        public string StartupFolder = "";


        public MainWindow()
        {
            InitializeComponent();

            _backgroundWorker = new BackgroundWorker();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _backgroundWorker.DoWork += new DoWorkEventHandler(Worker_DoWork);
            _backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(Worker_ProgressChanged);
            _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerCompleted);
            _backgroundWorker.WorkerReportsProgress = true;

            if (StartupFolder != "")
            {
                txtDirName.Text = StartupFolder;

                btnArchive_Click(btnArchive, new RoutedEventArgs(Button.ClickEvent));
            }

        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.Description = "Select folder";
            fbd.SelectedPath = @"C:\";
            fbd.ShowNewFolderButton = false;
            System.Windows.Forms.DialogResult dr = fbd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                //選択されたフォルダ
                txtDirName.Text = fbd.SelectedPath;
            }
        }

        private void btnArchive_Click(object sender, RoutedEventArgs e)
        {
            DirTool dir = new DirTool();


            _dirs = dir.GetAllSubDirectories(txtDirName.Text);
            string error_dir = "";

            int i = 0;
            try
            {
                foreach (string dirName in _dirs)
                {
                    error_dir = dirName;
                    if (dir.IsPicturesFolder(dirName))
                    {
                        i++;
                    }
                }
            }
            catch(PathTooLongException)
            {
                MessageBox.Show($"Path too long:{error_dir}");
                return;
            }


            progBar.Minimum = 0;
            progBar.Maximum = i;


            this.btnArchive.IsEnabled = false;
            this.btnBrowse.IsEnabled = false;
            _backgroundWorker.RunWorkerAsync(this);

        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int i = 0;
            DirTool dir = new DirTool();
            foreach (string dirName in _dirs)
            {
                if (dir.IsPicturesFolder(dirName))
                {
                    dir.createZip(dirName);
                    i++;
                    _backgroundWorker.ReportProgress(i);
                }
            }
        }


        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progBar.Value = e.ProgressPercentage;
        }


        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.btnArchive.IsEnabled = true;
            this.btnBrowse.IsEnabled = true;

            if (StartupFolder != "")
            {
                Application.Current.Shutdown();
            }


        }
    } 
}
