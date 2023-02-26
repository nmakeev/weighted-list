using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class WeightedListTests
    {
        // [Test]
        // public void WeightedList_SelectManyItems_PreserveProbability()
        // {
        //     //Arrange
        //     var items = new List<Entry<int>>
        //     {
        //         new Entry<int>(0, 1.0),
        //         new Entry<int>(1, 2.0),
        //         new Entry<int>(2, 0.5)
        //     };
        //     var weightedList = new WeightedList<int>();
        //     foreach (var entry in items)
        //     {
        //         weightedList.Add(entry.Value, entry.Weight);
        //     }
        //
        //     //Act
        //     const int repetitionCount = 1000000;
        //     var result = new int[3];
        //     for (var i = 0; i < repetitionCount; i++)
        //     {
        //         var randomElement = weightedList.GetRandomElement();
        //         result[randomElement]++;
        //     }
        //
        //     //Assert
        //     var actual = result.Select(item => item * 1f / repetitionCount).ToArray();
        //     
        //     var total = 0.0f;
        //     foreach (var entry in items)
        //     {
        //         total += (float)entry.Weight;
        //     }
        //     var expected = items.Select(item => (float)item.Weight / total).ToArray();
        //     
        //     Assert.AreEqual(actual.Length, expected.Length);
        //     for (var i = 0; i < actual.Length; i++)
        //     {
        //         Assert.IsTrue(Helpers.FloatEquals(actual[i], expected[i], 0.001f));
        //     }
        // }
        
        [Test]
        public void WeightedList_SelectOnlyOneItem_IfThereIsOnlyOneItemWithWeight()
        {
            var seed = Random.Range(int.MinValue, int.MaxValue);
            Random.InitState(seed);
            var elementsCount = Random.Range(5, 100);
            var elementWithMaxWeight = Random.Range(0, elementsCount);

            //Arrange
            var weightedList = new WeightedList<int>(seed);
            for (var i = 0; i < elementsCount; i++)
            {
                var weight = i == elementWithMaxWeight ? 1.0 : 0.0;
                weightedList.Add(i, weight);
            }

            //Act
            const int repetitionCount = 1_000_000;
            var result = new int[elementsCount];
            for (var i = 0; i < repetitionCount; i++)
            {
                var randomElement = weightedList.GetRandomElement();
                result[randomElement]++;
            }

            //Assert
            for (var i = 0; i < result.Length; i++)
            {
                var expected = i == elementWithMaxWeight ? repetitionCount : 0;
                Assert.AreEqual(expected, result[i], $"Seed = {seed}");
            }
        }
        
        [Test]
        public void WeightedList_RandomElementsExceptOne_IfThereIsElementWithZeroWeight()
        {
            var seed = Random.Range(int.MinValue, int.MaxValue);
            Random.InitState(seed);
            var elementsCount = Random.Range(5, 100);
            var elementWithZeroWeight = Random.Range(0, elementsCount);

            //Arrange
            var weightedList = new WeightedList<int>(seed);
            //foreach (var entry in items)
            for (var i = 0; i < elementsCount; i++)
            {
                var weight = i == elementWithZeroWeight ? 0.0 : Random.Range(1.0f, 10.0f);
                weightedList.Add(i, weight);
            }

            //Act
            const int repetitionCount = 1_000_000;
            var result = new int[elementsCount];
            for (var i = 0; i < repetitionCount; i++)
            {
                var randomElement = weightedList.GetRandomElement();
                result[randomElement]++;
            }

            //Assert
            for (var i = 0; i < result.Length; i++)
            {
                if (i == elementWithZeroWeight)
                {
                    Assert.AreEqual(0, result[i], $"Seed = {seed}");
                    continue;
                }
                
                Assert.NotZero(result[i], $"Seed = {seed}");
            }
        }
    }
}
