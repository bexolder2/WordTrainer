using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WordTrainer.Database.DbOperations;
using WordTrainer.Database.Dictionary;
using WordTrainer.Models.DbModels;
using WordTrainer.Models.Interfaces;
using WordTrainer.ViewModels.Base;
using WordTrainer.ViewModels.Base.Commands;

namespace WordTrainer.ViewModels
{
    public class WordTranslationViewModel : BaseViewModel, IWordTranslationViewModel
    {
        private ObservableCollection<Word> words;
        private ObservableCollection<Word> usedWords;
        private List<KeyValuePair<string, string>> previousGenerationCache;
        private static readonly Random rnd = new();
        private static readonly List<int> randomGeneratorCache = new() { 0, 1, 2, 3 };
        private static List<int> randomGeneratorCacheBuffer = new();
        private Word currentWord;
        private Dictionary<int, string> translationList;

        #region properties

        public Word CurrentWord
        {
            get => currentWord;
            set => Set(ref currentWord, value);
        }

        public Dictionary<int, string> TranslationList
        {
            get => translationList;
            set => Set(ref translationList, value);
        }

        public ICommand CheckTranslationCommand { get; private set; }
        public ICommand SkipWordCommand { get; private set; }
        public new ICommand NavigateToTrainingsMenuCommand { get; private set; }

        #endregion

        public WordTranslationViewModel()
        {
            CheckTranslationCommand = new LambdaCommand(CheckTranslationExecute);
            SkipWordCommand = new LambdaCommand(SkipWordExecute);
            NavigateToTrainingsMenuCommand = new LambdaCommand(NavigateToTrainingsMenuExecute);

            usedWords = new();
            currentWord = new();
            TranslationList = new();

            Task.Run(() => {
                words = new ObservableCollection<Word>(DbOperations.GetWords());
                LoadWords();
            });

#if TEST
            currentWord.TranslatedWord = "Caviar";
#endif
        }

        private void NavigateToTrainingsMenuExecute(object _)
        {
            //TODO: Show "Are u sure?" dialog
            Messenger.Instance.Send(new object());
        }

        /// <summary>
        /// If number of words users words <= 10 need use 2 translations from dictionary
        /// and for users words > 10 need use 1 translation from dictionary
        /// </summary>
        private void LoadWords() //TODO: refactor this method
        {
            if (words != null)
            {
                CurrentWord = LoadWord();
                usedWords.Add(currentWord);
                var bufferTranslationList = new Dictionary<int, string>();

                if (currentWord.NativeWord != null && currentWord.TranslatedWord != null)
                {
                    char letter = currentWord.NativeWord[0];
                    if (previousGenerationCache == null || previousGenerationCache?.First().Key[0] != letter)
                    {
                        previousGenerationCache = RUDictionary.Instance.GetWordsByLetter(letter);
                    }

                    int numberOfNeededWords = words.Count <= 10 ? 2 : 1;
                    int correctTranslationNumber = rnd.Next(0, 4);
                    randomGeneratorCacheBuffer.Clear();
                    randomGeneratorCacheBuffer.Add(correctTranslationNumber);
                    bufferTranslationList.Add(correctTranslationNumber, currentWord.NativeWord);

                    if (previousGenerationCache.Count >= numberOfNeededWords)
                    {
                        if (numberOfNeededWords == 1)
                        {
                            int mistakeIndex = GenerateMistakeIndex(randomGeneratorCacheBuffer);
                            randomGeneratorCacheBuffer.Add(mistakeIndex);
                            bufferTranslationList.Add(mistakeIndex, previousGenerationCache[rnd.Next(0, previousGenerationCache.Count)].Key);

                            int index = GenerateMistakeIndex(randomGeneratorCacheBuffer);
                            randomGeneratorCacheBuffer.Add(index);
                            var wordFromUserDictionary = LoadWord();
                            usedWords.Add(wordFromUserDictionary);
                            bufferTranslationList.Add(index, wordFromUserDictionary.NativeWord);
                        }
                        else if (numberOfNeededWords == 2)
                        {
                            int mistakeIndex1 = GenerateMistakeIndex(randomGeneratorCacheBuffer);
                            randomGeneratorCacheBuffer.Add(mistakeIndex1);
                            int mistakeIndex2 = GenerateMistakeIndex(randomGeneratorCacheBuffer);
                            randomGeneratorCacheBuffer.Add(mistakeIndex2);

                            int index = rnd.Next(0, previousGenerationCache.Count);
                            int index2 = index + 1 < previousGenerationCache.Count ? index + 1 : index - 1;
                            bufferTranslationList.Add(mistakeIndex1, previousGenerationCache[index].Key);
                            bufferTranslationList.Add(mistakeIndex2, previousGenerationCache[index2].Key);
                        }

                        int lastIndex = GenerateMistakeIndex(randomGeneratorCacheBuffer);
                        var lastWord = LoadWord();
                        usedWords.Add(lastWord);
                        bufferTranslationList.Add(lastIndex, lastWord.NativeWord);
                        TranslationList = new(bufferTranslationList);
                    }
                    else
                    {

                    }
                }
            }
        }

        private Word LoadWord()
        {
            Word result = new();
            try
            {
                var dictionary = words.Except(usedWords).ToList();
                result = dictionary[rnd.Next(0, dictionary.Count)];
            }
            catch (ArgumentOutOfRangeException) { }
            return result;
        }

        private int GenerateMistakeIndex(List<int> previousNumbers = null)
        {
            int number = default;
            if (previousNumbers != null)
            {
                var unusedNumbers = randomGeneratorCache.Except(previousNumbers).ToList();
                int numb = rnd.Next(0, unusedNumbers.Count);
                number = unusedNumbers[numb];
            }
            else
            {
                number = rnd.Next(0, 4);
            }

            return number;
        }

        private void CheckTranslationExecute(object selectedTranslation)
        {
            if (selectedTranslation != null)
            {
                string translation = string.Empty;
                if (selectedTranslation is KeyValuePair<int, string> word)
                {
                    translation = word.Value;
                }
                else if (selectedTranslation is string)
                {
                    translation = (string)selectedTranslation;
                }

                if (translation == currentWord.NativeWord)
                {
                    //TODO: logic for definition word as learned
                    //TODO: color animation
                }
                else
                {
                    //TODO: color animation
                }
                LoadWords();
            }
        }

        private void SkipWordExecute(object _)
        {
            usedWords.Remove(currentWord);
            LoadWords();
        }
    }
}
