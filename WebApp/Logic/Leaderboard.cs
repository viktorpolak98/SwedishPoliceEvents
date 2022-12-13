using System;
using System.Collections.Generic;
using WebApp.Repositories;
using WebApp.Models;

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
            int count = 1;
            if (NumberOfEventsDict.ContainsKey(eventType))
            {
                count += NumberOfEventsDict[eventType];
            }

            NumberOfEventsDict[eventType] = count;
        }

        public void AddCountEventLocation(string location)
        {
            int count = 1;
            if (NumberOfEventsLocationDict.ContainsKey(location))
            {
                count += NumberOfEventsLocationDict[location];
            }

            NumberOfEventsLocationDict[location] = count;
        }


    }
}
