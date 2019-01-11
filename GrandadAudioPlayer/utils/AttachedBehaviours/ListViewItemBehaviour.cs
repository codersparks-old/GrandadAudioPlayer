using System.Windows;
using System.Windows.Controls;

namespace GrandadAudioPlayer.Utils.AttachedBehaviours
{
    public class ListViewItemBehaviour
    {
        #region IsBroughtIntoViewWhenSelected

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
            if (!ReferenceEquals(sender, e.OriginalSource))
                return;

            if (e.OriginalSource is ListViewItem item)
                item.BringIntoView();
        }

        #endregion // IsBroughtIntoViewWhenSelected
    }
}
