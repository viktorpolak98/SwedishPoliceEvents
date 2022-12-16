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

            foreach (var val in enums)
            {
                Console.WriteLine(val);
            }

            Assert.NotNull(enums);

        }

        [Test]
        public void DisplayNameValue()
        {
            foreach (var item in Enum.GetValues(typeof(EventType)))
            {
                var displayName = EnumValuesHelper.GetAttribute<DisplayAttribute>((EventType)item);
                Console.WriteLine(displayName.Name);
            }

            Assert.Pass();
        }

        [Test]
        public void DisplayNameValues()
        {
            Dictionary<string, EventType> dict = EnumValuesHelper.ToDictionaryDisplayNameAsKey<EventType>();

            foreach (var val in dict)
            {
                Console.WriteLine($"{val.Key} {val.Value}");
            }

            Assert.AreEqual(dict.Count, 89);
        }

        [Test]
        public void OrderedDictionaryDescendingTest()
        {
            Dictionary<EventType, int> UnorderedDict = new Dictionary<EventType, int>
            {
                { EventType.Alkohollagen, 4 },
                { EventType.Brand, 2 },
                { EventType.Fjällräddning, 5 },
                { EventType.Inbrott, 2 },
                { EventType.Knivlagen, 1 }
            };

            UnorderedDict = DictionaryHelper.DictionaryToValueSortedByDescendingDictionary(UnorderedDict);

            List<int> orderedList = new List<int>();

            foreach (var val in UnorderedDict)
            {
                Console.WriteLine(val.Value);
                orderedList.Add(val.Value);
            }

            for (int i = 0; i < orderedList.Count - 1; i++)
            {
                Assert.GreaterOrEqual(orderedList[i], orderedList[i + 1]);
            }

            Assert.Pass();

        }

        [Test]
        public void OrderedDictionaryAscendingTest()
        {
            Dictionary<EventType, int> UnorderedDict = new Dictionary<EventType, int>
            {
                { EventType.Alkohollagen, 4 },
                { EventType.Brand, 2 },
                { EventType.Fjällräddning, 5 },
                { EventType.Inbrott, 2 },
                { EventType.Knivlagen, 1 }
            };

            UnorderedDict = DictionaryHelper.DictionaryToValueSortedByAscendingDictionary(UnorderedDict);

            List<int> orderedList = new List<int>();

            foreach (var val in UnorderedDict)
            {
                Console.WriteLine(val.Value);
                orderedList.Add(val.Value);
            }

            for (int i = 0; i < orderedList.Count - 1; i++)
            {
                Assert.GreaterOrEqual(orderedList[i + 1], orderedList[i]);
            }

            Assert.Pass();

        }

        [Test]
        public void UpdateCountTest()
        {
            Dictionary<EventType, int> Dict = new Dictionary<EventType, int>
            {
                { EventType.Alkohollagen, 4 }
            };

            DictionaryHelper.UpdateIntValue(Dict, EventType.Alkohollagen);

            Assert.AreEqual(Dict[EventType.Alkohollagen], 5);
        }

        [Test]
        public void AddCountTest()
        {
            Dictionary<EventType, int> Dict = new Dictionary<EventType, int>();

            DictionaryHelper.UpdateIntValue(Dict, EventType.Alkohollagen, 2);

            Assert.AreEqual(Dict.Count, 1);
            Assert.AreEqual(Dict[EventType.Alkohollagen], 2);
        }
    }
}
