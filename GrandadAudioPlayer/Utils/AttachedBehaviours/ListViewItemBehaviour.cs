using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GrandadAudioPlayer.Utils.AttachedBehaviours
{
    class ListViewItemBehaviour
    {
        #region IsBroughtIntoViewWhenSelected

        public static bool GetIsBroughtIntoViewWhenSelected(ListViewItem listViewItem)
        {
            return (bool)listViewItem.GetValue(IsBroughtIntoViewWhenSelectedProperty);
        }

        public static void SetIsBroughtIntoViewWhenSelected(
            ListViewItem listViewItem, bool value)
        {
            listViewItem.SetValue(IsBroughtIntoViewWhenSelectedProperty, value);
        }

        public static readonly DependencyProperty IsBroughtIntoViewWhenSelectedProperty =
            DependencyProperty.RegisterAttached(
                "IsBroughtIntoViewWhenSelected",
                typeof(bool),
                typeof(ListViewItemBehaviour),
                new UIPropertyMetadata(false, OnIsBroughtIntoViewWhenSelectedChanged));

        static void OnIsBroughtIntoViewWhenSelectedChanged(
            DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            ListViewItem item = depObj as ListViewItem;
            if (item == null)
                return;

            if (e.NewValue is bool == false)
                return;

            if ((bool)e.NewValue)
                item.Selected += OnListViewItemSelected;
            else
                item.Selected -= OnListViewItemSelected;
        }

        static void OnListViewItemSelected(object sender, RoutedEventArgs e)
        {
            // Only react to the Selected event raised by the ListViewItem
            // whose IsSelected property was modified. Ignore all ancestors
            // who are merely reporting that a descendant's Selected fired.
            if (!Object.ReferenceEquals(sender, e.OriginalSource))
                return;

            ListViewItem item = e.OriginalSource as ListViewItem;
            if (item != null)
                item.BringIntoView();
        }

        #endregion // IsBroughtIntoViewWhenSelected
    }
}
