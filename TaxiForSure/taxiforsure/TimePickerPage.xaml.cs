using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace TaxiforSure
{
    public partial class TimePickerPage : PhoneApplicationPage, IDateTimePickerPage
    {
        public TimePickerPage()
        {
            InitializeComponent();
            List<int> hours24 = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
            List<int> hours12 = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            List<string> minutes = new List<string>() { "00", "15", "30", "45" };
            List<string> ampm = new List<string>() { "AM", "PM" };
            if (Value == null) Value = RoundUp(DateTime.Now.Add(TimeSpan.FromHours(1)), TimeSpan.FromMinutes(15));
            String min2 = Value.Value.Minute.ToString();
            if (min2 == "0") min2 = "00";
            this.MinutesLoopingSelector.DataSource = new ListLoopingDataSource<string>() { Items = minutes, SelectedItem = min2 };
            this.AMPMLoopingSelector.DataSource = new ListLoopingDataSource<string>() { Items = ampm, SelectedItem = Value.Value.Hour > 12 ? "PM" : "AM" };
            if (DateTimeFormatInfo.CurrentInfo.ShortTimePattern.Contains("H"))
            {
                this.HoursLoopingSelector.DataSource = new ListLoopingDataSource<int>() { Items = hours24, SelectedItem = Value.Value.Hour };
                this.AMPMLoopingSelector.Visibility = Visibility.Collapsed;
            }
            else
            {
                int h12 = Value.Value.Hour;
                if (h12 > 12) h12 -= 12;
                if (h12 == 0) h12 = 12;
                this.HoursLoopingSelector.DataSource = new ListLoopingDataSource<int>() { Items = hours12, SelectedItem = h12 };
                this.AMPMLoopingSelector.Visibility = Visibility.Visible;
            }
        }
        private void ApplicationBarCancelButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
        DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

        }

        private void ApplicationBarDoneButton_Click(object sender, EventArgs e)
        {
            int hour = (int)((ListLoopingDataSource<int>)this.HoursLoopingSelector.DataSource).SelectedItem;
            int minutes = int.Parse((string)((ListLoopingDataSource<string>)this.MinutesLoopingSelector.DataSource).SelectedItem);
            if (this.AMPMLoopingSelector.Visibility == Visibility.Visible)
            {
                string ampm = (string)((ListLoopingDataSource<string>)this.AMPMLoopingSelector.DataSource).SelectedItem;
                if (ampm == "PM" && hour != 12)
                { hour += 12; }

                if (ampm == "AM" && hour == 12) hour = 0;
            }
            Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minutes, 0);

            NavigationService.GoBack();
        }
        public void SetFlowDirection(FlowDirection flowDirection)
        {
            // throw new NotImplementedException();

        }

        public DateTime? Value { get; set; }
    }
    public abstract class LoopingDataSourceBase : ILoopingSelectorDataSource
    {
        private object selectedItem;

        #region ILoopingSelectorDataSource Members

        public abstract object GetNext(object relativeTo);

        public abstract object GetPrevious(object relativeTo);

        public object SelectedItem
        {
            get
            {
                return this.selectedItem;
            }
            set
            {
                // this will use the Equals method if it is overridden for the data source item class
                if (!object.Equals(this.selectedItem, value))
                {
                    // save the previously selected item so that we can use it 
                    // to construct the event arguments for the SelectionChanged event
                    object previousSelectedItem = this.selectedItem;
                    this.selectedItem = value;
                    // fire the SelectionChanged event
                    this.OnSelectionChanged(previousSelectedItem, this.selectedItem);
                }
            }
        }

        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        protected virtual void OnSelectionChanged(object oldSelectedItem, object newSelectedItem)
        {
            EventHandler<SelectionChangedEventArgs> handler = this.SelectionChanged;
            if (handler != null)
            {
                handler(this, new SelectionChangedEventArgs(new object[] { oldSelectedItem }, new object[] { newSelectedItem }));
            }
        }

        #endregion
    }
    public class ListLoopingDataSource<T> : LoopingDataSourceBase
    {
        private LinkedList<T> linkedList;
        private List<LinkedListNode<T>> sortedList;
        private NodeComparer nodeComparer;
        private IComparer<T> comparer;

        public ListLoopingDataSource() { }

        public IEnumerable<T> Items
        {
            get
            {
                return this.linkedList;
            }
            set
            {
                this.SetItemCollection(value);
            }
        }

        private void SetItemCollection(IEnumerable<T> collection)
        {
            this.linkedList = new LinkedList<T>(collection);
            this.sortedList = new List<LinkedListNode<T>>(this.linkedList.Count);

            // initialize the linked list with items from the collections
            LinkedListNode<T> currentNode = this.linkedList.First;
            while (currentNode != null)
            {
                this.sortedList.Add(currentNode);
                currentNode = currentNode.Next;
            }

            IComparer<T> comparer = this.comparer;
            if (comparer == null)
            {
                // if no comparer is set use the default one if available
                if (typeof(IComparable<T>).IsAssignableFrom(typeof(T)))
                {
                    comparer = Comparer<T>.Default;
                }
                else
                {
                    throw new InvalidOperationException("There is no default comparer for this type of item. You must set one.");
                }
            }

            this.nodeComparer = new NodeComparer(comparer);
            this.sortedList.Sort(this.nodeComparer);
        }

        public IComparer<T> Comparer
        {
            get
            {
                return this.comparer;
            }
            set
            {
                this.comparer = value;
            }
        }

        public override object GetNext(object relativeTo)
        {
            // find the index of the node using binary search in the sorted list
            int index = this.sortedList.BinarySearch(new LinkedListNode<T>((T)relativeTo), this.nodeComparer);
            if (index < 0)
            {
                return default(T);
            }

            // get the actual node from the linked list using the index
            LinkedListNode<T> node = this.sortedList[index].Next;
            if (node == null)
            {
                // if there is no next node get the first one
                node = this.linkedList.First;
            }
            return node.Value;
        }

        public override object GetPrevious(object relativeTo)
        {
            int index = this.sortedList.BinarySearch(new LinkedListNode<T>((T)relativeTo), this.nodeComparer);
            if (index < 0)
            {
                return default(T);
            }
            LinkedListNode<T> node = this.sortedList[index].Previous;
            if (node == null)
            {
                // if there is no previous node get the last one
                node = this.linkedList.Last;
            }
            return node.Value;
        }

        private class NodeComparer : IComparer<LinkedListNode<T>>
        {
            private IComparer<T> comparer;

            public NodeComparer(IComparer<T> comparer)
            {
                this.comparer = comparer;
            }

            #region IComparer<LinkedListNode<T>> Members

            public int Compare(LinkedListNode<T> x, LinkedListNode<T> y)
            {
                return this.comparer.Compare(x.Value, y.Value);
            }

            #endregion
        }
    }
}