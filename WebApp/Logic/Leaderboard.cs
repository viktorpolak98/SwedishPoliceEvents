using System;
using System.Collections.Generic;
using WebApp.Repositories;
using WebApp.Models;
using WebApp.HelperFunctions;

namespace WebApp.Logic
{
    public class Leaderboard
    {
        public Dictionary<EventType, int> NumberOfEventsDict { get; set; }
        public Dictionary<string, int> NumberOfEventsLocationDict { get; set; }

        public Leaderboard()
        {
            NumberOfEventsDict = new Dictionary<EventType, int>();
            NumberOfEventsLocationDict = new Dictionary<string, int>();
        }

        public void ClearDictionaries()
        {
            NumberOfEventsDict.Clear();
            NumberOfEventsLocationDict.Clear();
        }

        public void AddCountEvent(EventType eventType)
        {
            DictionaryHelper.UpdateIntValue(NumberOfEventsDict, eventType);
        }

        public void AddCountEventLocation(string location)
        {
            DictionaryHelper.UpdateIntValue(NumberOfEventsLocationDict, location);
        }

        public void SortDictionariesDescending()
        {
            NumberOfEventsDict = DictionaryHelper.DictionaryToValueSortedByDescendingDictionary(NumberOfEventsDict);
            NumberOfEventsLocationDict = DictionaryHelper.DictionaryToValueSortedByDescendingDictionary(NumberOfEventsLocationDict);
        }


    }
}
