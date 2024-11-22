using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Homework
{
    public class AreaTests
    {
        [TestCase(10, 0)]
        [TestCase(5, 5)]
        [TestCase(1, 10)]
        public void Instantiate(
            int capacity,
            int currentCount
        )
        {
            //Act:
            ConverterArea area = new(capacity, currentCount);

            //Assert:
            Assert.AreEqual(capacity, area.Capacity);
            Assert.AreEqual(currentCount, area.CurrentCount);
        }

        [TestCase(0, 5)]
        [TestCase(5, -3)]
        [TestCase(-3, 5)]
        [TestCase(3, -5)]
        public void WhenInvalidArgumentsThenException(int capacity, int count)
        {
            //Assert:
            Assert.Catch<ArgumentException>(() => { _ = new ConverterArea(capacity, count); });
        }

        [TestCaseSource(nameof(CanAddResourcesCases))]
        public bool CanAddResources(ConverterArea area, int count)
        {
            return area.CanAddResources(count);
        }

        private static IEnumerable<TestCaseData> CanAddResourcesCases()
        {
            yield return new TestCaseData(
                new ConverterArea(capacity: 5, currentCount: 0),
                3
            ).Returns(true).SetName("Empty area");

            yield return new TestCaseData(
                new ConverterArea(capacity: 1, currentCount: 0),
                2
            ).Returns(true).SetName("Too much");

            yield return new TestCaseData(
                new ConverterArea(capacity: 5, currentCount: 5),
                1
            ).Returns(false).SetName("Is full");
        }

        [TestCaseSource(nameof(CanRemoveResourcesCases))]
        public bool CanRemoveResources(ConverterArea area, int count)
        {
            return area.CanRemoveResources(count);
        }

        private static IEnumerable<TestCaseData> CanRemoveResourcesCases()
        {
            yield return new TestCaseData(
                new ConverterArea(capacity: 5, currentCount: 0),
                3
            ).Returns(false).SetName("Empty area");

            yield return new TestCaseData(
                new ConverterArea(capacity: 2, currentCount: 2),
                3
            ).Returns(false).SetName("Too much");

            yield return new TestCaseData(
                new ConverterArea(capacity: 5, currentCount: 5),
                5
            ).Returns(true).SetName("Is full");
        }

        [TestCaseSource(nameof(AddResourcesSuccessfulCases))]
        public void AddResourcesSuccessful(ConverterArea area, int count)
        {
            //Arrange:
            int addedCount = 0;
            int previousCount = area.CurrentCount;

            area.OnAdded += (p) => { addedCount = p; };

            //Act:
            bool success = area.AddResources(count, out int change);

            //Assert:
            Assert.IsTrue(success);
            Assert.AreEqual(change, 0);
            Assert.AreEqual(previousCount + count, area.CurrentCount);
            Assert.AreEqual(addedCount, count);
        }

        private static IEnumerable<TestCaseData> AddResourcesSuccessfulCases()
        {
            yield return new TestCaseData(
                new ConverterArea(capacity: 5, currentCount: 0),
                3
            ).SetName("Empty area");

            yield return new TestCaseData(
                new ConverterArea(capacity: 5, currentCount: 4),
                1
            ).SetName("Make full 4/5");

            yield return new TestCaseData(
                new ConverterArea(capacity: 100, currentCount: 50),
                50
            ).SetName("Make full 50/100");
        }
        
        [TestCaseSource(nameof(AddResourcesWithChangeCases))]
        public void AddResourcesWithChange(ConverterArea area, int count, int expectedChange)
        {
            //Arrange:
            int addedCount = 0;
            int previousCount = area.CurrentCount;

            area.OnAdded += (p) => { addedCount = p; };

            //Act:
            bool success = area.AddResources(count, out int change);

            //Assert:
            Assert.IsTrue(success);
            Assert.AreEqual(expectedChange, change);
            Assert.AreEqual(previousCount + (count-change), area.CurrentCount);
            Assert.AreEqual(addedCount, count-change);
        }

        private static IEnumerable<TestCaseData> AddResourcesWithChangeCases()
        {
            yield return new TestCaseData(
                new ConverterArea(capacity: 5, currentCount: 0),
                60,
                55
            ).SetName("Empty area 0/5, change=55");

            yield return new TestCaseData(
                new ConverterArea(capacity: 5, currentCount: 4),
                5,
                4
            ).SetName("Make full 4/5, change=4");

            yield return new TestCaseData(
                new ConverterArea(capacity: 100, currentCount: 50),
                50,
                0
            ).SetName("Make full 50/100, change=0");
        }

        [TestCaseSource(nameof(RemoveResourcesSuccessfulCases))]
        public void RemoveResourcesSuccessful(ConverterArea area, int count)
        {
            //Arrange:
            int removedCount = 0;
            int previousCount = area.CurrentCount;

            area.OnRemoved += (p) => { removedCount = p; };

            //Act:
            bool success = area.RemoveResources(count);

            //Assert:
            Assert.IsTrue(success);
            Assert.AreEqual(previousCount - count, area.CurrentCount);
            Assert.AreEqual(removedCount, count);
        }

        private static IEnumerable<TestCaseData> RemoveResourcesSuccessfulCases()
        {
            yield return new TestCaseData(
                new ConverterArea(capacity: 5, currentCount: 5),
                5
            ).SetName("Make full empty 5->0");

            yield return new TestCaseData(
                new ConverterArea(capacity: 50, currentCount: 35),
                35
            ).SetName("Make not full empty 35->0");

            yield return new TestCaseData(
                new ConverterArea(capacity: 5, currentCount: 3),
                2
            ).SetName("Take some 3->1");
        }
    }
}