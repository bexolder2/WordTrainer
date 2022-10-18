using System.Windows.Controls;
using System.Windows.Input;
using WordTrainer.Models.Interfaces;
using WordTrainer.ViewModels;

namespace WordTrainer.Pages
{
    public partial class WordTranslationPage : Page, IWordTranslationPage
    {
        public WordTranslationPage(IWordTranslationViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;
            RootGrid.KeyUp += WordTranslationPage_KeyUp;
            Loaded += WordTranslationPage_Loaded;
        }

        private void WordTranslationPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            RootGrid.Focus();
        }

        private void WordTranslationPage_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            var vm = (WordTranslationViewModel)DataContext;
            switch (e.Key)
            {
                case Key.D1:
                case Key.F1:

                    break;
                case Key.D2:
                case Key.F2:
                    
                    break;
                case Key.D3:
                case Key.F3:
                    
                    break;
                case Key.D4:
                case Key.F4:
                    
                    break;
                case Key.Back:
                case Key.Delete:
                case Key.Escape:
                    vm.NavigateToTrainingsMenuCommand.Execute(null);
                    break;
            }
        }
    }
}
