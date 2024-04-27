using NUnit.Framework;
using WebApp.HelperFunctions;
using WebApp.Models.PoliceEvent;
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

            Assert.NotNull(enums);

        }

        [Test]
        public void DisplayNameValue()
        {
            foreach (var item in Enum.GetValues(typeof(EventType)))
            {
                var displayName = EnumValuesHelper.GetAttribute<DisplayAttribute>((EventType)item);
                Assert.NotNull(displayName);
            }

            Assert.Pass();
        }

        [Test]
        public void DisplayNameValues()
        {
            Dictionary<string, EventType> dict = EnumValuesHelper.ToDictionaryDisplayNameAsKey<EventType>();

            foreach (var val in dict)
            {
                Assert.NotNull(val.Key);
                Assert.NotNull(val.Value);
            }

            Assert.AreEqual(dict.Count, 89);
        }

        [Test]
        public void OrderedDictionaryDescendingTest()
        {
            Dictionary<EventType, int> UnorderedDict = new()
            {
                { EventType.Alkohollagen, 4 },
                { EventType.Brand, 2 },
                { EventType.Fjällräddning, 5 },
                { EventType.Inbrott, 2 },
                { EventType.Knivlagen, 1 }
            };

            UnorderedDict = DictionaryHelper.DictionaryToValueSortedByDescendingDictionary(UnorderedDict);

            List<int> orderedList = new();

            foreach (var val in UnorderedDict)
            {
                orderedList.Add(val.Value);
                Assert.NotNull(val.Value);
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
            Dictionary<EventType, int> UnorderedDict = new()
            {
                { EventType.Alkohollagen, 4 },
                { EventType.Brand, 2 },
                { EventType.Fjällräddning, 5 },
                { EventType.Inbrott, 2 },
                { EventType.Knivlagen, 1 }
            };

            UnorderedDict = DictionaryHelper.DictionaryToValueSortedByAscendingDictionary(UnorderedDict);

            List<int> orderedList = new();

            foreach (var val in UnorderedDict)
            {
                orderedList.Add(val.Value);
                Assert.NotNull (val.Value);
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
            Dictionary<EventType, int> Dict = new()
            {
                { EventType.Alkohollagen, 4 }
            };

            DictionaryHelper.UpdateIntValue(Dict, EventType.Alkohollagen);

            Assert.AreEqual(Dict[EventType.Alkohollagen], 5);
        }

        [Test]
        public void AddCountTest()
        {
            Dictionary<EventType, int> Dict = new();

            DictionaryHelper.UpdateIntValue(Dict, EventType.Alkohollagen, 2);

            Assert.AreEqual(Dict.Count, 1);
            Assert.AreEqual(Dict[EventType.Alkohollagen], 2);
        }
    }
}
