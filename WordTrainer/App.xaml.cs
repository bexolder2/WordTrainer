using System;
using System.Windows;
using WordTrainer.ViewModels;

namespace WordTrainer
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        [STAThread]
        public static void Main()
        {
            App app = new();

            MainViewModel mainViewModel = new();

            MainWindow window = new(mainViewModel);

            app.Run(window);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }
    }
}
