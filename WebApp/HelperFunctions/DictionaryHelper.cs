using System.Collections.Generic;
using System.Linq;

namespace WebApp.HelperFunctions
{
    /// <summary>
    /// A helper class for dictionary functionality
    /// </summary>
    public class DictionaryHelper  
    {
        /// <summary>
        /// Sorts a dictionary in a descending order based on value
        /// </summary>
        /// <typeparam name="TK">Generic key</typeparam>
        /// <typeparam name="TVal">Generic value</typeparam>
        /// <param name="Dict">Dictionary to sort</param>
        /// <returns></returns>
        public static Dictionary<TK, TVal> DictionaryToValueSortedByDescendingDictionary<TK, TVal>(Dictionary<TK, TVal> Dict) 
        {
            Dict = Dict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            return Dict;
        }

        /// <summary>
        /// Sorts a dictionary in a ascending order based on value
        /// </summary>
        /// <typeparam name="TK">Generic key</typeparam>
        /// <typeparam name="TVal">Generic value</typeparam>
        /// <param name="Dict">Dictionary to sort</param>
        /// <returns></returns>
        public static Dictionary<TK, TVal> DictionaryToValueSortedByAscendingDictionary<TK, TVal>(Dictionary<TK, TVal> Dict)
        {
            Dict = Dict.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            return Dict;
        }

        /// <summary>
        /// Updates an integer value from a generic key. By default value is updated by 1
        /// </summary>
        /// <typeparam name="TK">Generic key</typeparam>
        /// <param name="Dict">Dictionary to update</param>
        /// <param name="Key">Key to update value</param>
        /// <param name="amount">The amount to update by. By default 1</param>
        public static void UpdateIntValue<TK>(Dictionary<TK, int> Dict, TK Key, int amount = 1)
        {
            if (Dict.ContainsKey(Key))
            {
                amount += Dict[Key];
            }

            Dict[Key] = amount;
        }
    }
}
