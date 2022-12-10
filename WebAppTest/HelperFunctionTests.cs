using NUnit.Framework;
using WebApp.HelperFunctions;
using WebApp.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace WebAppTest
{
    class HelperFunctionTests
    {
        [Test]
        public void EnumValues()
        {
            var enums = EnumValuesHelper.GetValues<EventType>();

            foreach(var val in enums)
            {
                Console.WriteLine($"{val}");
            }

            Assert.NotNull(enums);
            
        }

        [Test]
        public void DisplayNameValue()
        {
            foreach (var item in Enum.GetValues(typeof(EventType)))
            {
                var displayName = EnumValuesHelper.GetAttribute<DisplayAttribute>((EventType)item);
                Console.WriteLine($"{displayName.Name}");
            }

            Assert.Pass();
        }
        
        [Test]
        public void DisplayNameValues()
        {
            Dictionary<string, EventType> dict = EnumValuesHelper.ToDictionaryDisplayNameAsKey<EventType>();

            foreach(var val in dict)
            {
                Console.WriteLine($"{val.Key} {val.Value}");
            }
            
            Assert.AreEqual(dict.Count, 89);
        }
    }
}
