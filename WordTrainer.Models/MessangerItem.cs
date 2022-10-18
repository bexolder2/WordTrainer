using System;
using WordTrainer.Models.Interfaces;

namespace WordTrainer.Models
{
    public class MessangerItem
    {
        public Type Type { get; set; }
        public IViewModel ViewModel { get; set; }
        public Action<object> Handler { get; set; }

        public MessangerItem() { }

        public MessangerItem(Type type, IViewModel viewModel, Action<object> handler)
        {
            Type = type;
            ViewModel = viewModel;
            Handler = handler;
        }
    }
}
