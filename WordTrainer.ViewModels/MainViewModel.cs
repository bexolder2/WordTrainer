using System.Collections.ObjectModel;
using WordTrainer.Database.DbOperations;
using WordTrainer.Models.DbModels;
using WordTrainer.Models.Enums;
using WordTrainer.Models.Interfaces;
using WordTrainer.ViewModels.Base;

namespace WordTrainer.ViewModels
{
    public class MainViewModel : BaseViewModel, IMainViewModel
    {
        #region fields
        private ObservableCollection<Word>? words;
        #endregion

        #region properties
        public ObservableCollection<Word>? Words
        {
            get => words;
            set => Set(ref words, value);
        }
        #endregion

        public MainViewModel()
        {
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
    }
}
