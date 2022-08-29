using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WordTrainer.Models.Interfaces;

namespace WordTrainer.Dialogs.Base
{
    public class BaseDialog : IDialog //TODO: Implement dialog service
    {
        public void CloseDialog()
        {
            throw new NotImplementedException();
        }

        public void ShowDialog()
        {
            if (Application.Current != null)
            {
                var root = Application.Current.MainWindow as UIElement;
                var dialogGrid = FindRootGrid(root);

                if (dialogGrid != null)
                {
                    //dialogGrid.Children.Add();

                    dialogGrid.Visibility = Visibility.Visible;
                }
            } 
        }

        private Grid FindRootGrid(UIElement root)
        {
            Grid grid = new();
            if (root != null)
            {
                List<Grid> grids = new();
                FindChildren(grids, root);
                if (grids.Any())
                {
                    grid = grids.FirstOrDefault(control => control.Name == "DialogGrid");
                }
            }
            return grid;
        }

        private void FindChildren<T>(List<T> results, DependencyObject startNode) where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(startNode);
            for (int i = 0; i < count; i++)
            {
                DependencyObject current = VisualTreeHelper.GetChild(startNode, i);
                if ((current.GetType()).Equals(typeof(T)) || (current.GetType().GetTypeInfo().IsSubclassOf(typeof(T))))
                {
                    T asType = (T)current;
                    results.Add(asType);
                }
                FindChildren<T>(results, current);
            }
        }
    }
}
