using System;
using System.Windows.Input;
using WordTrainer.Models.DbModels;

namespace WordTrainer.Models.Interfaces
{
    public interface IAddWordViewModel : IViewModel
    {
        ICommand TranslateCommand { get; set; }

        ICommand AddWordCommand { get; set; }

        ICommand CancelCommand { get; set; }

        Word Word { get; set; }

        event EventHandler<Word> CloseDialog;
    }
}
