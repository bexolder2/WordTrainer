using System.Windows;
using System.Windows.Data;
using WordTrainer.Models.Interfaces;
using WordTrainer.Pages;
using WordTrainer.ViewModels;

namespace WordTrainer
{
    public partial class MainWindow : Window, IMainView
    {
        public MainWindow(IMainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            var trainingViewModel = Factory.Factory.Instance.CreateInstance<BaseTrainingViewModel>(typeof(ITrainingViewModel));

            var dictionaryPage = Factory.Factory.Instance.CreateFrameworkElement<DictionaryPage>(typeof(IDictionaryPage));
            var trainingPage = Factory.Factory.Instance.CreateFrameworkElement<TrainingPage>(typeof(ITrainingPage), trainingViewModel);
            var settingsPage = Factory.Factory.Instance.CreateFrameworkElement<SettingsPage>(typeof(ISettingsPage));

            dictionaryPage.DataContext = viewModel;

            Binding frameContentBinding = new Binding("CurrentTraining");
            frameContentBinding.Source = trainingViewModel;
            trainingPage.SetBinding(TrainingPage.FrameContentProperty, frameContentBinding);

            DictionaryFrame.Content = dictionaryPage;
            TrainingFrame.Content = trainingPage;
            SettingsFrame.Content = settingsPage;
        }
    }
}
