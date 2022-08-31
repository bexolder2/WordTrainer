using System.Windows;
using WordTrainer.Models.Interfaces;
using WordTrainer.Pages;

namespace WordTrainer
{
    public partial class MainWindow : Window, IMainView
    {
        public MainWindow(IMainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            var dictionaryPage = Factory.Factory.Instance.CreateFrameworkElement<DictionaryPage>(typeof(IDictionaryPage));
            var trainingPage = Factory.Factory.Instance.CreateFrameworkElement<TrainingPage>(typeof(ITrainingPage));
            var settingsPage = Factory.Factory.Instance.CreateFrameworkElement<SettingsPage>(typeof(ISettingsPage));

            dictionaryPage.DataContext = viewModel;

            DictionaryFrame.Content = dictionaryPage;
            TrainingFrame.Content = trainingPage;
            SettingsFrame.Content = settingsPage;
        }
    }
}
