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
    }
}
