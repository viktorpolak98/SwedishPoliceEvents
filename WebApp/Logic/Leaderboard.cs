using System.Collections.Generic;
using WebApp.Models.PoliceEvent;
using WebApp.HelperFunctions;

namespace WebApp.Logic
{
    /// <summary>
    /// Leaderboard class to calculate the most occuring types of events and most frequent location
    /// </summary>
    public class Leaderboard
    {
        public Dictionary<EventType, int> NumberOfEventsDict { get; set; }
        public Dictionary<string, int> NumberOfEventsLocationDict { get; set; }

        public Leaderboard()
        {
            NumberOfEventsDict = new Dictionary<EventType, int>();
            NumberOfEventsLocationDict = new Dictionary<string, int>();
        }

        /// <summary>
        /// Clears dictionaries 
        /// </summary>
        public void ClearDictionaries()
        {
            NumberOfEventsDict.Clear();
            NumberOfEventsLocationDict.Clear();
        }

        /// <summary>
        /// Updates the count for an event
        /// </summary>
        /// <param name="eventType">The specific event to update</param>
        public void AddCountEvent(EventType eventType)
        {
            DictionaryHelper.UpdateIntValue(NumberOfEventsDict, eventType);
        }

        /// <summary>
        /// Updates the count for a location
        /// </summary>
        /// <param name="location">The specific location to update</param>
        public void AddCountEventLocation(string location)
        {
            DictionaryHelper.UpdateIntValue(NumberOfEventsLocationDict, location);
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
            NumberOfEventsDict = DictionaryHelper.DictionaryToValueSortedByAscendingDictionary(NumberOfEventsDict);
            NumberOfEventsLocationDict = DictionaryHelper.DictionaryToValueSortedByAscendingDictionary(NumberOfEventsLocationDict);
        }

        /// <summary>
        /// Sorts the dictionaries in a descending order
        /// </summary>
        private void SortDictionariesDescending()
        {
            NumberOfEventsDict = DictionaryHelper.DictionaryToValueSortedByDescendingDictionary(NumberOfEventsDict);
            NumberOfEventsLocationDict = DictionaryHelper.DictionaryToValueSortedByDescendingDictionary(NumberOfEventsLocationDict);
        }


    }
}
