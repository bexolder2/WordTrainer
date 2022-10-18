using System.Windows;
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
            Application.Current.MainWindow.KeyDown += WndKeyDown;
        }

        private void WndKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            var vm = (WordTranslationViewModel)DataContext;
            switch (e.Key)
            {
                case Key.D1:
                case Key.F1:
                    vm.CheckTranslationCommand.Execute(TranslationsList.Items[0]);
                    break;
                case Key.D2:
                case Key.F2:
                    vm.CheckTranslationCommand.Execute(TranslationsList.Items[1]);
                    break;
                case Key.D3:
                case Key.F3:
                    vm.CheckTranslationCommand.Execute(TranslationsList.Items[2]);
                    break;
                case Key.D4:
                case Key.F4:
                    vm.CheckTranslationCommand.Execute(TranslationsList.Items[3]);
                    break;
                case Key.Delete:
                case Key.Escape:
                    vm.NavigateToTrainingsMenuCommand.Execute(null);
                    break;
            }
        }
    }
}
