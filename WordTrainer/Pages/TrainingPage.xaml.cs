using System.Windows;
using System.Windows.Controls;
using WordTrainer.Models.Enums;
using WordTrainer.Models.Interfaces;
using WordTrainer.ViewModels;

namespace WordTrainer.Pages
{
    public partial class TrainingPage : UserControl, ITrainingPage
    {
        #region Dependency properties
        public static readonly DependencyProperty FrameContentProperty = 
            DependencyProperty.Register("FrameContent", 
                                        typeof(Training),
                                        typeof(TrainingPage),
                                        new PropertyMetadata(Training.None, FrameContentChanged));

        public Training FrameContent
        {
            get => (Training)GetValue(FrameContentProperty);
            set => SetValue(FrameContentProperty, value);
        }
        #endregion
        
        private static WordTranslationViewModel wordTranslationVm => Factory.Factory.Instance.CreateInstance<WordTranslationViewModel>(typeof(IWordTranslationViewModel));

        private static WordTranslationPage wordTranslationPage => Factory.Factory.Instance.CreateFrameworkElement<WordTranslationPage>(typeof(IWordTranslationPage), wordTranslationVm);

        public TrainingPage(ITrainingViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private static void FrameContentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            switch ((Training)args.NewValue)
            {
                case Training.WordTranslation:
                    ((TrainingPage)obj).TrainingFrame.Content = wordTranslationPage;
                    ((TrainingPage)obj).TrainingFrame.Visibility = Visibility.Visible;
                    break;
                case Training.TranslationWord:
                    break;
                case Training.WordBuilder:
                    break;
                case Training.Sprint:
                    break;
                case Training.Listening:
                    break;
                case Training.None:
                    ((TrainingPage)obj).TrainingFrame.Visibility = Visibility.Collapsed;
                    break;
            }
        }
    }
}
