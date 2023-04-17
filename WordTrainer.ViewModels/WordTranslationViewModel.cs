using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WordTrainer.Database.DbOperations;
using WordTrainer.Database.Dictionary;
using WordTrainer.Models.DbModels;
using WordTrainer.Models.Enums;
using WordTrainer.Models.Interfaces;
using WordTrainer.ViewModels.Base;
using WordTrainer.ViewModels.Base.Commands;

namespace WordTrainer.ViewModels
{
    public class WordTranslationViewModel : BaseViewModel, IWordTranslationViewModel
    {
        private static readonly int totalWordsCount = 4;
        private ObservableCollection<Word> words;
        private ObservableCollection<Word> usedWords;
        private List<KeyValuePair<string, string>> previousGenerationCache;
        private List<string> usedFromCache;
        private static readonly Random rnd = new();
        private static readonly List<int> randomGeneratorCache = new() { 0, 1, 2, 3 };
        private Word currentWord;
        private List<string> translationList;
        private AnswerStatus answerStatus = AnswerStatus.None;

        #region properties

        public AnswerStatus AnswerStatus
        {
            get => answerStatus;
            set => Set(ref answerStatus, value);
        }

        public Word CurrentWord
        {
            get => currentWord;
            set => Set(ref currentWord, value);
        }

        public List<string> TranslationList
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
            usedFromCache = new();
            currentWord = new();
            TranslationList = new();

            Task.Run(() => {
                //TODO: fix adding same words on start 
                words = new ObservableCollection<Word>(DbOperations.GetWords());
                LoadWords();
            });
        }

        private void NavigateToTrainingsMenuExecute(object _)
        {
            //TODO: Show "Are u sure?" dialog
            Messenger.Instance.Send(new object());
        }

        /// <summary>
        /// If number of users words <= 10 need use 2 translations from dictionary
        /// and for users words > 10 need use 1 translation from dictionary
        /// </summary>
        private void LoadWords() //TODO: refactor this method
        {
            if (words != null)
            {
                TranslationList.Clear();
                AnswerStatus = AnswerStatus.None;
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

                    int numberWordsFromDictionary = words.Count <= 10 ? 2 : 1;
                    int correctTranslationIndex = rnd.Next(0, 4);

                    if (previousGenerationCache.Count >= numberWordsFromDictionary)
                    {
                        usedFromCache.Clear();
                        for (int i = 0; i < numberWordsFromDictionary; i++)
                        {
                            //Check is word uniq
                            bool isUniq = true;
                            do
                            {
                                string incorrectWord = previousGenerationCache[rnd.Next(0, previousGenerationCache.Count)].Key;
                                if (usedFromCache.Count == 0 || !usedFromCache.Contains(incorrectWord))
                                {
                                    usedFromCache.Add(incorrectWord);
                                    TranslationList.Add(incorrectWord);
                                }
                                else if (usedFromCache.Contains(incorrectWord))
                                {
                                    isUniq = false;
                                }
                            }
                            while (!isUniq);
                        }

                        usedFromCache.Clear();
                        for (int i = 0; i < totalWordsCount - 1 - numberWordsFromDictionary; i++)
                        {
                            //TODO: move this logic to LoadWord()
                            //Check is word uniq
                            //bool isUniq = true;
                            //do
                            //{
                            //    string correctWord = LoadWord().NativeWord;
                            //    if (usedFromCache.Count == 0 || !usedFromCache.Contains(correctWord))
                            //    {
                            //        usedFromCache.Add(correctWord);
                            //        TranslationList.Add(correctWord);
                            //    }
                            //    else if (usedFromCache.Contains(correctWord))
                            //    {
                            //        isUniq = false;
                            //    }
                            //}
                            //while (!isUniq);
                            TranslationList.Add(LoadWord().NativeWord);
                        }

                        //Sort incorrect translations by random letter
                        int sorter = rnd.Next(0, TranslationList.OrderBy(str => str.Length).First().Length);
                        TranslationList = TranslationList.OrderBy(str => str[sorter]).ToList();
                        TranslationList.Insert(correctTranslationIndex, currentWord.NativeWord);
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
                    AnswerStatus = AnswerStatus.Correct;
                    //TODO: logic for definition word as learned
                }
                else
                {
                    AnswerStatus = AnswerStatus.Wrong;
                }
                Task.Run(() => {
                    Thread.Sleep(500);
                    LoadWords();
                });
            }
        }

        private void SkipWordExecute(object _)
        {
            AnswerStatus = AnswerStatus.None;
            usedWords.Remove(currentWord);
            LoadWords();
        }
    }
}
