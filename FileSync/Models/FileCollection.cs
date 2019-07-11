using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.Models
{
    public class FileCollection<T> : ObservableCollection<T>
    {
        private bool _suppressNotification;

        public FileCollection() { }

        public FileCollection(IEnumerable<T> items) : base(items)
        {

        }

        public void AddRange(IEnumerable<T> items)
        {
            if(items == null)
            {
                return;
            }

            _suppressNotification = true;

            var list = items.ToList();

            foreach(var item in list)
            {
                Add(item);
            }

            _suppressNotification = false;

            if(list.Any())
            {
                OnCollectionChangedMultiItem(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, list));
            }
        }

        public void AddRange(params T[] items)
        {
            AddRange((IEnumerable<T>)items);
        }

        public void ReplaceWithRange(IEnumerable<T> items)
        {
            Items.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            AddRange(items);
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            _suppressNotification = true;

            var remove = items.Where(x => Items.Contains(x)).ToList();

            foreach(var item in remove)
            {
                Remove(item);
            }

            _suppressNotification = false;

            if(remove.Any())
            {
                OnCollectionChangedMultiItem(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public override event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnCollectionChangedMultiItem(NotifyCollectionChangedEventArgs e)
        {
            var handlers = CollectionChanged;

            if(handlers == null)
            {
                return;
            }

            foreach(NotifyCollectionChangedEventHandler handler in handlers.GetInvocationList())
            {
                if(handler.Target is ICollectionView collectionView)
                {
                    collectionView.Refresh();
                }
                else
                {
                    handler(this, e);
                }
            }
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if(_suppressNotification)
            {
                return;
            }

            base.OnCollectionChanged(e);

            CollectionChanged?.Invoke(this, e);
        }
    }
}
