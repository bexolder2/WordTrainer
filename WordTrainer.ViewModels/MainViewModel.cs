using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WordTrainer.Database.DbOperations;
using WordTrainer.Factory;
using WordTrainer.Models.DbModels;
using WordTrainer.Models.Enums;
using WordTrainer.Models.Interfaces;
using WordTrainer.ViewModels.Base;
using WordTrainer.ViewModels.Base.Commands;

namespace WordTrainer.ViewModels
{
    public class MainViewModel : BaseViewModel, IMainViewModel
    {
        #region fields
        private ObservableCollection<Word>? words;

        private Window addWordDialog;
        #endregion

        #region properties
        public ObservableCollection<Word>? Words
        {
            get => words;
            set => Set(ref words, value);
        }

        public ICommand AddWordCommand { get; private set; }
        #endregion

        public MainViewModel()
        {
            AddWordCommand = new LambdaCommand(AddWordExecute);

            InitializeWorlds();
        }

        #region initializers
        private void InitializeWorlds()
        {
#if Debug||Release
            Words = new(DbOperations.GetWords());
#elif Test
            Words = new();
            Words.Add(new Word { Id = 0, NativeWord = "тест", TranslatedWord = "test", Status = WordStatus.Learned});
            Words.Add(new Word { Id = 0, NativeWord = "кот", TranslatedWord = "cat", Status = WordStatus.NeedToLearn});
            Words.Add(new Word { Id = 0, NativeWord = "бобек", TranslatedWord = "dog", Status = WordStatus.NeedToRepeat});
#endif
        }
        #endregion

        #region commands
        private void AddWordExecute(object obj) //TODO: refactor, create dialog service
        {
            var viewModel = Factory.Factory.Instance.CreateInstance<AddWordViewModel>(typeof(IAddWordViewModel));
            viewModel.CloseDialog += CloseAddWordDialog;
            addWordDialog = (Window)Factory.Factory.Instance.CreateFrameworkElement<FrameworkElement>(typeof(IAddWordDialog));
            addWordDialog.DataContext = viewModel;
            addWordDialog.ShowDialog();
        }

        private void CloseAddWordDialog(object? sender, Word word)
        {
            if (word != null)
            {
                Words?.Add(word);
            }
            addWordDialog.Close();
        }
        #endregion
    }
}
