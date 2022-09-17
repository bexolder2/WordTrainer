using System;
using System.Collections.Generic;
using System.Linq;
using WordTrainer.Models;
using WordTrainer.Models.Interfaces;

namespace WordTrainer.ViewModels
{
    public class Messenger
    {
        #region Singletone
        private static Lazy<Messenger> instance = new();

        public static Messenger Instance => instance.Value;
        #endregion

        private List<MessangerItem> subscribers;

        public Messenger() 
        {
            subscribers = new List<MessangerItem>();
        }

        public void Register<T>(IViewModel vm, Action<object> handler)
        {
            subscribers.Add(new MessangerItem(typeof(T), vm, handler));
        }

        public void Send<T>(T data)
        {
            var subscribers_ = subscribers.FindAll(x => x.Type == data?.GetType());
            if (subscribers_.Any())
            {
                subscribers_.ForEach((item) => {
                    item.Handler.Invoke(data);
                });
            }
        }
    }
}
