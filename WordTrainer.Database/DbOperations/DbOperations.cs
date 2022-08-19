using System;
using System.Collections.Generic;
using System.Linq;
using WordTrainer.Models.DbModels;

namespace WordTrainer.Database.DbOperations
{
    /// <summary>
    /// Provides CRUD operations
    /// </summary>
    public class DbOperations
    {
        #region Singletone
        private readonly Lazy<DbOperations> instance = new();

        public DbOperations Instance => instance.Value;
        #endregion

        private static readonly ApplicationDbContext context;

        static DbOperations()
        {
            context = new ApplicationDbContext();
        }

        public static void AddOrUpdateWord(Word model, int id = -1)
        {
            using (ApplicationDbContext db = new())
            {
                Word? wordFromDb = null;
                if (id != -1)
                {
                    wordFromDb = db.UsersWords.FirstOrDefault(word => word.Id == id);
                }

                if (wordFromDb != null)
                {
                    wordFromDb = null;
                    DeleteWord(id);
                    AddOrUpdateWord(model);
                }
                else
                {
                    db.UsersWords.Add(model);
                }
                db.SaveChanges();
            }
        }

        public static List<Word> GetWords()
        {
            using (ApplicationDbContext db = new())
            {
                return db.UsersWords.ToList();
            }
        }

        public static void DeleteWord(int id)
        {
            using (ApplicationDbContext db = new())
            {
                var model = db.UsersWords.FirstOrDefault(word => word.Id == id);
                db.Remove(model);
                db.SaveChanges();
            }
        }
    }
}
