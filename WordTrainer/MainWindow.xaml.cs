using System.Windows;
using WordTrainer.Models.Interfaces;

namespace WordTrainer
{
    public partial class MainWindow : Window, IMainView
    {
        public MainWindow(IMainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
