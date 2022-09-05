using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
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
        public ICommand DeleteWordCommand { get; private set; }
        #endregion

        public MainViewModel()
        {
            AddWordCommand = new LambdaCommand(AddWordExecute);
            DeleteWordCommand = new LambdaCommand(DeleteWordExecute);

            InitializeWorlds();
        }

        #region initializers
        private void InitializeWorlds()
        {
#if Debug||Release
            Words = new(DbOperations.GetWords());
#elif Test
            Words = new();
            Words.Add(new Word { NativeWord = "тест", TranslatedWord = "test", Status = WordStatus.Learned});
            Words.Add(new Word { NativeWord = "кот", TranslatedWord = "cat", Status = WordStatus.NeedToLearn});
            Words.Add(new Word { NativeWord = "бобек", TranslatedWord = "dog", Status = WordStatus.NeedToRepeat});
            DbOperations.AddOrUpdateWord(Words[0]);
            DbOperations.AddOrUpdateWord(Words[1]);
            DbOperations.AddOrUpdateWord(Words[2]);
#endif
        }
        #endregion

        #region commands
        private void DeleteWordExecute(object word)
        {
            if (word is Word)
            {
                DbOperations.DeleteWord(((Word)word).Id);
                Words?.Remove((Word)word);
            }
        }

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
                DbOperations.AddOrUpdateWord(word);
            }
            addWordDialog.Close();
        }
        #endregion
    }
}
