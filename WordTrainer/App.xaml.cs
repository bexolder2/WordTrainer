using System;
using System.Windows;
using WordTrainer.Dialogs;
using WordTrainer.Models.Interfaces;
using WordTrainer.Pages;
using WordTrainer.ViewModels;

namespace WordTrainer
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            InitializeTypes();
        }

        [STAThread]
        public static void Main()
        {
            App app = new();

            MainViewModel mainViewModel = new();

            MainWindow window = new(mainViewModel);

            app.Run(window);
        }

        private void InitializeTypes()
        {
            Factory.Factory.InitializeFactory();
            Factory.Factory.Instance.RegisterType(typeof(IAddWordDialog), typeof(AddWordDialog));

            Factory.Factory.Instance.RegisterType(typeof(IAddWordViewModel), typeof(AddWordViewModel));

            Factory.Factory.Instance.RegisterType(typeof(IDictionaryPage), typeof(DictionaryPage));
            Factory.Factory.Instance.RegisterType(typeof(ITrainingPage), typeof(TrainingPage));
            Factory.Factory.Instance.RegisterType(typeof(ISettingsPage), typeof(SettingsPage));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }
    }
}
