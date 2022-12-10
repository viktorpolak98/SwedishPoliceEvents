using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace WebApp.HelperFunctions
{
    public static class EnumValuesHelper 
    {
        public static IEnumerable<TEnum> GetValues<TEnum>()
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }


        public static Dictionary<string, TEnum> ToDictionaryDisplayNameAsKey<TEnum>()
        {
            var returnDict = new Dictionary<string, TEnum>();
            foreach (var item in Enum.GetValues(typeof(TEnum)))
            {
                returnDict.Add(GetAttribute<DisplayAttribute>((Enum)item).Name, (TEnum)item);
            }

            return returnDict;
        }

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
