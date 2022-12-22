using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace WebApp.HelperFunctions
{
    /// <summary>
    /// Generic enum helper class
    /// </summary>
    public static class EnumValuesHelper 
    {
        /// <summary>
        /// Generic method to return all values from an enum
        /// </summary>
        /// <typeparam name="TEnum">Generic enum</typeparam>
        /// <returns></returns>
        public static IEnumerable<TEnum> GetValues<TEnum>()
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }

        /// <summary>
        /// Generic method to create a dictionary with the System.ComponentModel.DataAnnotations.DisplayName attribute as key 
        /// </summary>
        /// <typeparam name="TEnum">Generic enum</typeparam>
        /// <returns></returns>
        public static Dictionary<string, TEnum> ToDictionaryDisplayNameAsKey<TEnum>()
        {
            var returnDict = new Dictionary<string, TEnum>();
            foreach (var item in Enum.GetValues(typeof(TEnum)))
            {
                returnDict.Add(GetAttribute<DisplayAttribute>((Enum)item).Name, (TEnum)item);
            }

            return returnDict;
        }

        /// <summary>
        /// Generic method to get the first attribute from an enum
        /// </summary>
        /// <typeparam name="TAttribute">Generic attribute</typeparam>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
        where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }
    }
}
