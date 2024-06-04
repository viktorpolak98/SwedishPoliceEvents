using System.Collections.Generic;

namespace WebApp.Models.Shared
{
    public class ItemContainer <T>
    {
        public LinkedList<T> Items { get; set; }
        public int MaxNumberOfItems { get; } 

        public ItemContainer(int maxNumberOfItems)
        {
            Items = new LinkedList<T>();
            MaxNumberOfItems = maxNumberOfItems;
        }

        public void AddItem(T item)
        {
            if(Items.Count >= MaxNumberOfItems)
            {
                Items.RemoveLast();
            }

            Items.AddFirst(item);
        }

    }
}
