using System.Collections.Generic;
using WebApp.Models.PoliceEvent;
using WebApp.HelperFunctions;

namespace WebApp.Logic
{
    /// <summary>
    /// Leaderboard class to calculate the most occuring types of events and most frequent location
    /// </summary>
    public class Leaderboard<T>
    {
        public Dictionary<T, int> NumberOfTypeDict { get; set; }
        public Dictionary<string, int> NumberOfLocationDict { get; set; }

        public Leaderboard()
        {
            NumberOfTypeDict = [];
            NumberOfLocationDict = [];
        }

        /// <summary>
        /// Clears dictionaries 
        /// </summary>
        public void ClearDictionaries()
        {
            NumberOfTypeDict.Clear();
            NumberOfLocationDict.Clear();
        }

        /// <summary>
        /// Updates the count for an event
        /// </summary>
        /// <param name="type">The specific event to update</param>
        public void AddCountEvent(T type)
        {
            NumberOfTypeDict.TryGetValue(type, out var count);
            NumberOfTypeDict.Add(type, count+1);
        }

        /// <summary>
        /// Updates the count for a location
        /// </summary>
        /// <param name="location">The specific location to update</param>
        public void AddCountEventLocation(string location)
        {
            NumberOfLocationDict.TryGetValue(location, out var count);
            NumberOfLocationDict.Add(location, count+1);
        }

        /// <summary>
        /// Determines in what order to sort the dictionaries (descending or ascending)
        /// </summary>
        /// <param name="Descending">True if descending order otherwise ascending order</param>
        public void SortDictionaries(bool Descending)
        {
            if (Descending)
            {
                SortDictionariesDescending();
                return;
            } 

            SortDictionariesAscending();
        }

        /// <summary>
        /// Sorts the dictionaries in a ascending order
        /// </summary>
        private void SortDictionariesAscending()
        {
            NumberOfTypeDict = DictionaryHelper.DictionaryToValueSortedByAscendingDictionary(NumberOfTypeDict);
            NumberOfLocationDict = DictionaryHelper.DictionaryToValueSortedByAscendingDictionary(NumberOfLocationDict);
        }

        /// <summary>
        /// Sorts the dictionaries in a descending order
        /// </summary>
        private void SortDictionariesDescending()
        {
            NumberOfTypeDict = DictionaryHelper.DictionaryToValueSortedByDescendingDictionary(NumberOfTypeDict);
            NumberOfLocationDict = DictionaryHelper.DictionaryToValueSortedByDescendingDictionary(NumberOfLocationDict);
        }


    }
}
