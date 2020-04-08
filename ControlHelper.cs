using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CheckBox = System.Windows.Controls.CheckBox;
using ListBox = System.Windows.Controls.ListBox;

namespace KcptunManager
{
    public static class ControlHelper
    {
        public static List<ListBoxItem> GetItems(this ListBox listBox)
        {
            var listBoxItems = new List<ListBoxItem>();
            foreach (var item in listBox.Items)
            {
                var listBoxItem = listBox.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;

                if (listBoxItem == null)
                    continue;

                listBoxItems.Add(listBoxItem);
            }

            return listBoxItems;
        }

        public static List<ListBoxItem> GetSelectedItems(this ListBox listBox)
        {
            var listBoxItems = new List<ListBoxItem>();
            foreach (var item in listBox.Items)
            {
                var listBoxItem = listBox.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;

                if (listBoxItem == null)
                    continue;

                var contentPresenter = FindVisualChild<ContentPresenter>(listBoxItem);
                var contentTemplate = contentPresenter.ContentTemplate;
                var checkBox = contentTemplate.FindName("CheckBox", contentPresenter) as CheckBox;

                if (checkBox == null)
                    continue;

                if (checkBox.IsChecked == true)
                    listBoxItems.Add(listBoxItem);
            }

            return listBoxItems;
        }

        public static void Checked(this ListBoxItem listBoxItem)
        {
            var contentPresenter = FindVisualChild<ContentPresenter>(listBoxItem);
            var contentTemplate = contentPresenter.ContentTemplate;
            if (contentTemplate.FindName("CheckBox", contentPresenter) is CheckBox checkBox)
                checkBox.IsChecked = true;
        }

        public static void UnChecked(this ListBoxItem listBoxItem)
        {
            var contentPresenter = FindVisualChild<ContentPresenter>(listBoxItem);
            var contentTemplate = contentPresenter.ContentTemplate;
            if (contentTemplate.FindName("CheckBox", contentPresenter) is CheckBox checkBox)
                checkBox.IsChecked = false;
        }

        public static TChildItem FindVisualChild<TChildItem>(DependencyObject obj) where TChildItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child is TChildItem item)
                {
                    return item;
                }

                TChildItem childOfChild = FindVisualChild<TChildItem>(child);
                if (childOfChild != null)
                    return childOfChild;
            }

            return null;
        }
    }
}