using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace FileSync.Models
{
    public class FileCollection<T> : ObservableCollection<T>
    {
        // Flag used during bulk adds or removals
        private bool _suppressNotification;

        public FileCollection() { }

        public FileCollection(IEnumerable<T> items) : base(items) { }

        public void AddRange(IEnumerable<T> items)
        {
            // If nothing was passed, return early
            if(items == null)
            {
                return;
            }

            // Set the flag to prevent unnecessary notifications
            _suppressNotification = true;

            // Turn the item collection into a list
            var list = items.ToList();

            // For each item in the list
            foreach(var item in list)
            {
                // Add it to this collection
                Add(item);
            }

            // Reset the flag to its normal state
            _suppressNotification = false;

            // If any items were added
            if(list.Any())
            {
                // Raise an event to signify this collection has changed
                OnCollectionChangedMultiItem(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, list));
            }
        }

        public void AddRange(params T[] items)
        {
            // Cast the array to an enumerable and pass it to the main AddRange method
            AddRange((IEnumerable<T>)items);
        }

        public void ReplaceWithRange(IEnumerable<T> items)
        {
            // Clear the underlying items collection
            Items.Clear();

            // Raise an event to signify the underlying collection has been reset/cleared
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            
            // Pass the given items to the AddRange method
            AddRange(items);
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            // Set the flag to prevent unnecessary notifications
            _suppressNotification = true;

            // Find all instances in the underlying collection of items to be removed
            var remove = items.Where(x => Items.Contains(x)).ToList();

            // For each removable item
            foreach(var item in remove)
            {
                // Remove it from the underlying collection
                Remove(item);
            }

            // Reset the flag to its normal state
            _suppressNotification = false;

            // If any items were selected for removal
            if(remove.Any())
            {
                // Raise an event to signify the underlying collection has changed
                OnCollectionChangedMultiItem(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public new void Remove(T item)
        {
            if(Items.Contains(item))
            {
                Items.Remove(item);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public override event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnCollectionChangedMultiItem(NotifyCollectionChangedEventArgs e)
        {
            // Get the event handlers for the CollectionChanged event
            var handlers = CollectionChanged;

            // If there are no handlers, return early
            if(handlers == null)
            {
                return;
            }

            // For each handler in the handlers list
            foreach(NotifyCollectionChangedEventHandler handler in handlers.GetInvocationList())
            {
                // If the target is a collection view (like the listview in the UI)
                if(handler.Target is ICollectionView collectionView)
                {
                    // Refresh the view
                    collectionView.Refresh();
                }
                else
                {
                    // Otherwise, pass the arguments to the handler for processing
                    handler(this, e);
                }
            }
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            // If the flag is set, do nothing and return early
            if(_suppressNotification)
            {
                return;
            }

            // Pass the arguments to the base
            base.OnCollectionChanged(e);

            // Invoke the CollectionChanged event to signal the handlers
            CollectionChanged?.Invoke(this, e);
        }
    }
}
