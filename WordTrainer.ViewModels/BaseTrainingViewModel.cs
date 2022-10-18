using System.Windows.Input;
using WordTrainer.Models.Enums;
using WordTrainer.Models.Interfaces;
using WordTrainer.ViewModels.Base;
using WordTrainer.ViewModels.Base.Commands;

namespace WordTrainer.ViewModels
{
    public class BaseTrainingViewModel : BaseViewModel, ITrainingViewModel
    {
        private Training currentTraining;
        private bool isTrainingMenuVisible;
        private static bool isInitialized = false;

        #region properties
        public bool IsTrainingMenuVisible
        {
            get => isTrainingMenuVisible;
            set => Set(ref isTrainingMenuVisible, value);
        }

        public Training CurrentTraining
        {
            get => currentTraining;
            set => Set(ref currentTraining, value);
        }

        public ICommand NavigateToTrainingCommand { get; private set; }
        public ICommand NavigateToTrainingsMenuCommand { get; protected set; }
        #endregion

        public BaseTrainingViewModel()
        {
            if (!isInitialized)
            {
                NavigateToTrainingCommand = new LambdaCommand(NavigateToTrainingExecute);
                NavigateToTrainingsMenuCommand = new LambdaCommand(NavigateToTrainingsMenuExecute);

                CurrentTraining = Training.None;
                IsTrainingMenuVisible = true;
                isInitialized = true;

                Messenger.Instance.Register<object>(this, NavigateToTrainingsMenuExecute);
            } 
        }

        protected void NavigateToTrainingsMenuExecute(object _)
        {
            IsTrainingMenuVisible = true;
            CurrentTraining = Training.None;
        }

        private void NavigateToTrainingExecute(object source)
        {
            if (source != null && source is Training navigationSource)
            {
                CurrentTraining = navigationSource;
                IsTrainingMenuVisible = false;
            }
        }
    }
}
