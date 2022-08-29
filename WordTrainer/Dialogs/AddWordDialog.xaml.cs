using System.Windows;
using System.Windows.Input;
using WordTrainer.Models.Interfaces;

namespace WordTrainer.Dialogs
{
    public partial class AddWordDialog : Window, IAddWordDialog
    {
        public AddWordDialog()
        {
            InitializeComponent();
            AddWordBorder.MouseLeftButtonDown += NewWordText_MouseLeftButtonDown;
        }

        private void NewWordText_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
