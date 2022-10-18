using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WordTrainer.Models.DbModels;

namespace WordTrainer.Database.Dictionary
{
    public class RUDictionary
    {
        #region Singletone
        private static Lazy<RUDictionary> instance = new();

        public static RUDictionary Instance => instance.Value;
        #endregion

        private readonly string wordsPath = @"Dictionary\ru_words.json";
        private static SortedDictionary<string, string> words { get; set; }

        public RUDictionary() { }

        public void InitializeRUWords()
        {
            words = new SortedDictionary<string, string>();
            try
            {
                var text = File.ReadAllText(wordsPath);
                if (!string.IsNullOrEmpty(text))
                {
                    JArray items = JArray.Parse(text);
                    var deserializedWords = items.ToObject<List<DictionaryWord>>();
                    if (deserializedWords != null)
                    {
                        //Parallel.ForEach(deserializedWords, word =>
                        //{
                        //    if (!words.ContainsKey(word.Word))
                        //    {
                        //        words?.Add(word.Word, word.Noun);
                        //    }                          
                        //});
                        //TODO: fix NullReferenceException in Parallel.ForEach
                        foreach (var word in deserializedWords)
                        {
                            if (!words.ContainsKey(word.Word))
                            {
                                words?.Add(word.Word, word.PartOfSpeech);
                            }
                        }
                    }
                }
            }
            catch (FileNotFoundException) { }
            catch (InvalidCastException) { }
            catch (NullReferenceException) { }
            catch (Exception) { }
        } 

        public List<KeyValuePair<string, string>> GetWordsByLetter(char letter)
        {
            return words.Where(word => word.Key[0] == letter).ToList();
        }


        public KeyValuePair<string, string> GetWordByNoun()
        {
            throw new NotImplementedException();
        }
    }
}
