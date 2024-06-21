using System.Collections.Generic;
using WebApp.Models.PoliceEvent;

namespace WebApp.Logic
{
    /// <summary>
    /// Leaderboard class to calculate the most occuring types of events and most frequent location
    /// </summary>
    public class Leaderboard
    {
        public Dictionary<string, int> NumberOfTypeDict { get; set; }
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
        public void AddCountEvent(string type)
        {
            NumberOfTypeDict.TryGetValue(type, out var count);
            NumberOfTypeDict[type] = count + 1;
        }

        /// <summary>
        /// Updates the count for a location
        /// </summary>
        /// <param name="location">The specific location to update</param>
        public void AddCountEventLocation(string location)
        {
            NumberOfLocationDict.TryGetValue(location, out var count);
            NumberOfLocationDict[location] = count + 1;
        }
    }
}
