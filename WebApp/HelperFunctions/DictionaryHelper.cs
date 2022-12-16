using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using System.Collections.Generic;
using System.Linq;
using WebApp.Models;

namespace WebApp.HelperFunctions
{
    public class DictionaryHelper  
    {
        public static Dictionary<TK, TVal> DictionaryToValueSortedByDescendingDictionary<TK, TVal>(Dictionary<TK, TVal> Dict) 
        {
            Dict = Dict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            return Dict;
        }

        public static Dictionary<TK, TVal> DictionaryToValueSortedByAscendingDictionary<TK, TVal>(Dictionary<TK, TVal> Dict)
        {
            Dict = Dict.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            return Dict;
        }

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
