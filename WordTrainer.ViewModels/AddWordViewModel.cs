using System;
using System.Windows.Input;
using WordTrainer.Models.DbModels;
using WordTrainer.Models.Interfaces;
using WordTrainer.ViewModels.Base;
using WordTrainer.ViewModels.Base.Commands;

namespace WordTrainer.ViewModels
{
    public class AddWordViewModel : BaseViewModel, IAddWordViewModel
    {
        #region fields
        private Word word;
        #endregion

        #region properties
        public ICommand TranslateCommand { get; set; }
        public ICommand AddWordCommand { get; set; }
        public ICommand CancelCommand { get; set; }
       
        public Word Word 
        { 
            get => word;
            set => Set(ref word, value);
        }
        #endregion

        public event EventHandler<Word> CloseDialog;

        public AddWordViewModel()
        {
            TranslateCommand = new LambdaCommand(TranslateExecute);
            AddWordCommand = new LambdaCommand(AddWordExecute);
            CancelCommand = new LambdaCommand(CancelExecute);
            Word = new();
        }

        #region commands
        private void TranslateExecute(object _) //TODO: Implement translate using some API
        {
            throw new NotImplementedException();
        }

        private void AddWordExecute(object _)
        {
            CloseDialog?.Invoke(this, Word);
        }

        private void CancelExecute(object _)
        {
            Word = null;
            CloseDialog?.Invoke(this, Word);
        }
        #endregion

    }
}
