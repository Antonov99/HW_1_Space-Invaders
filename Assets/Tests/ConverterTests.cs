using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Homework
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class ConverterTests
    {
        [TestCase(2, 1, 1f)]
        [TestCase(3, 1, 2f)]
        [TestCase(1, 3, 1.5f)]
        [TestCase(2, 1, 4f)]
        public void Instantiate(
            int inCountToConvert,
            int outCountAfterConvert,
            float timeToConvert
        )
        {
            //Arrange
            LoadingArea loadingArea = new LoadingArea(5, new Resource("A"));
            UnloadingArea unloadingArea = new UnloadingArea(5, new Resource("A"));
            
            //Act:
            var converter = new Converter(loadingArea, unloadingArea, inCountToConvert, outCountAfterConvert,
                timeToConvert);

            //Assert:
            Assert.AreEqual(loadingArea, converter.LoadingArea);
            Assert.AreEqual(unloadingArea, converter.UnloadingArea);
            Assert.AreEqual(inCountToConvert, converter.InCountToConvert);
            Assert.AreEqual(outCountAfterConvert, converter.OutCountAfterConvert);
            Assert.AreEqual(timeToConvert, converter.TimeToConvert);
        }

        [Test]
        public void WhenInvalidArgumentsThenException()
        {
            LoadingArea loadingArea = new LoadingArea(5, new Resource("A"));
            UnloadingArea unloadingArea = new UnloadingArea(5, new Resource("A"));

            //Assert:
            Assert.Catch<ArgumentException>(() => { _ = new Converter(loadingArea, unloadingArea, -4, 1, 1f); });
            Assert.Catch<ArgumentException>(() => { _ = new Converter(loadingArea, unloadingArea, 1, -2, 1f); });
            Assert.Catch<ArgumentException>(() => { _ = new Converter(loadingArea, unloadingArea, 1, 1, 0f); });
            Assert.Catch<NullReferenceException>(() => { _ = new Converter(null, unloadingArea, 1, 1, 0f); });
            Assert.Catch<NullReferenceException>(() => { _ = new Converter(loadingArea, null, 1, 1, 0f); });
        }
        
        [TestCaseSource(nameof(CanPutResourcesCases))]
        public bool CanPutResources(Converter converter, Resource resource, int count)
        {
            return converter.CanPutResources(resource, count);
        }

        private static IEnumerable<TestCaseData> CanPutResourcesCases()
        {
            yield return new TestCaseData(
                new Converter(new LoadingArea(5,new Resource("Wood")),new UnloadingArea(5,new Resource("Lambert")),2,1,1f),
                new Resource(name: "Wood"),
                3
            ).Returns(true).SetName("Empty area");

            yield return new TestCaseData(
                new Converter(new LoadingArea(5,new Resource("Wood")),new UnloadingArea(5,new Resource("Lambert")),2,1,1f),
                new Resource(name: "Wood"),
                6
            ).Returns(false).SetName("Too much");

            yield return new TestCaseData(
                new Converter(new LoadingArea(5,new Resource("Wood")),new UnloadingArea(5,new Resource("Lambert")),2,1,1f),
                new Resource(name: "Lambert"),
                2
            ).Returns(false).SetName("Not a target resource");
        }
        
        [TestCaseSource(nameof(CanTakeResourcesCases))]
        public bool CanTakeResources(Converter converter, Resource resource, int count)
        {
            return converter.CanTakeResources(resource, count);
        }

        private static IEnumerable<TestCaseData> CanTakeResourcesCases()
        {
            yield return new TestCaseData(
                new Converter(new LoadingArea(5,new Resource("Wood")),new UnloadingArea(5,new Resource("Lambert")),2,1,1f),
                new Resource(name: "Lambert"),
                3
            ).Returns(false).SetName("Empty area");

            yield return new TestCaseData(
                new Converter(new LoadingArea(5,new Resource("Wood")),new UnloadingArea(5,new Resource("Lambert")),2,1,1f),
                new Resource(name: "Lambert"),
                6
            ).Returns(false).SetName("Too much");
            
            yield return new TestCaseData(
                new Converter(new LoadingArea(5,new Resource("Wood")),new UnloadingArea(5,new Resource("Lambert"),5),2,1,1f),
                new Resource(name: "Lambert"),
                5
            ).Returns(true).SetName("Is full");
            
            yield return new TestCaseData(
                new Converter(new LoadingArea(5,new Resource("Wood")),new UnloadingArea(5,new Resource("Lambert"),3),2,1,1f),
                new Resource(name: "Lambert"),
                2
            ).Returns(true).SetName("Success");

            yield return new TestCaseData(
                new Converter(new LoadingArea(5,new Resource("Wood")),new UnloadingArea(5,new Resource("Lambert")),2,1,1f),
                new Resource(name: "Wood"),
                2
            ).Returns(false).SetName("Not a target resource");
        }
        
        [TestCaseSource(nameof(PutResourcesSuccessfulCases))]
        public void PutResourcesSuccessful(Converter converter, Resource resource, int count)
        {
            //Arrange:
            Resource puttedResource = null;
            int puttedCount = 0;

            converter.OnPut += (i, p) =>
            {
                puttedResource = i;
                puttedCount = p;
            };

            //Act:
            bool success = converter.PutResources(resource, count);

            //Assert:
            Assert.IsTrue(success);
            Assert.AreEqual(puttedResource, resource);
            Assert.AreEqual(puttedCount, count);
        }
        
        private static IEnumerable<TestCaseData> PutResourcesSuccessfulCases()
        {
            yield return new TestCaseData(
                new Converter(new LoadingArea(5,new Resource("Wood")),new UnloadingArea(5,new Resource("Lambert"),3),2,1,1f),
                new Resource(name: "Wood"),
                3
            ).SetName("Empty area");

            yield return new TestCaseData(
                new Converter(new LoadingArea(5,new Resource("Wood"),4),new UnloadingArea(5,new Resource("Lambert"),3),2,1,1f),
                new Resource(name: "Wood"),
                1
            ).SetName("Make full 4/5");

            yield return new TestCaseData(
                new Converter(new LoadingArea(100,new Resource("Wood"),50),new UnloadingArea(5,new Resource("Lambert"),3),2,1,1f),
                new Resource(name: "Wood"),
                50
            ).SetName("Make full 50/100");
        }
        
        [TestCaseSource(nameof(TakeResourcesSuccessfulCases))]
        public void TakeResourcesSuccessful(Converter converter, Resource resource, int count)
        {
            //Arrange:
            Resource takenResource = null;
            int takenCount = 0;

            converter.OnTake += (i, p) =>
            {
                takenResource = i;
                takenCount = p;
            };

            //Act:
            bool success = converter.TakeResources(resource, count);

            //Assert:
            Assert.IsTrue(success);
            Assert.AreEqual(takenResource, resource);
            Assert.AreEqual(takenCount, count);
        }
        
        private static IEnumerable<TestCaseData> TakeResourcesSuccessfulCases()
        {
            yield return new TestCaseData(
                new Converter(new LoadingArea(5,new Resource("Wood")),new UnloadingArea(5,new Resource("Lambert"),3),2,1,1f),
                new Resource(name: "Lambert"),
                3
            ).SetName("Take all");

            yield return new TestCaseData(
                new Converter(new LoadingArea(5,new Resource("Wood"),4),new UnloadingArea(5,new Resource("Lambert"),3),2,1,1f),
                new Resource(name: "Lambert"),
                1
            ).SetName("Take one");

            yield return new TestCaseData(
                new Converter(new LoadingArea(100,new Resource("Wood"),50),new UnloadingArea(100,new Resource("Lambert"),100),2,1,1f),
                new Resource(name: "Lambert"),
                50
            ).SetName("Take half 50/100");
        }
        
        [TestCaseSource(nameof(ConvertResourcesSuccessfulCases))]
        public void ConvertResourcesSuccessful(Converter converter, Resource resource, int count, int loadingAreaCount, int unloadingAreaCount)
        {
            //Arrange:
            List<Resource> convertedResources = new();
            int convertedCount = 0;
            
            converter.OnConvert += (i, p) =>
            {
                convertedResources.Add(i);
                convertedCount += p;
            };

            //Act:
            converter.StartConvert();

            //Assert:
            foreach (var convertedResource in convertedResources)
            {
                Assert.AreEqual(resource.Name,convertedResource.Name);
            }
            Assert.AreEqual(count, convertedCount);
            Assert.AreEqual(loadingAreaCount, converter.LoadingArea.CurrentCount);
            Assert.AreEqual(unloadingAreaCount, converter.UnloadingArea.CurrentCount);
        }
        
        private static IEnumerable<TestCaseData> ConvertResourcesSuccessfulCases()
        {
            yield return new TestCaseData(
                new Converter(new LoadingArea(5,new Resource("Wood"),5),new UnloadingArea(5,new Resource("Lambert"),0),2,1,1f),
                new Resource(name: "Lambert"),
                2,
                1,
                2
            ).SetName("Was empty, 5->2, final 2/5");

            yield return new TestCaseData(
                new Converter(new LoadingArea(5,new Resource("Wood"),1),new UnloadingArea(5,new Resource("Lambert"),0),2,1,1f),
                new Resource(name: "Lambert"),
                0,
                1,
                0
            ).SetName("Was empty, 1->0");

            yield return new TestCaseData(
                new Converter(new LoadingArea(100,new Resource("Wood"),50),new UnloadingArea(10,new Resource("Lambert"),10),2,1,1f),
                new Resource(name: "Lambert"),
                0,
                50,
                10
            ).SetName("Was full, 50->0");
            
            yield return new TestCaseData(
                new Converter(new LoadingArea(100,new Resource("Wood"),50),new UnloadingArea(10,new Resource("Lambert"),5),2,1,1f),
                new Resource(name: "Lambert"),
                5,
                40,
                10
            ).SetName("Was not full, 50->5");
        }

    }

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
            Resource targetResourceType = new Resource("A");
            BaseArea area = new(capacity, targetResourceType, currentCount);

            //Assert:
            Assert.AreEqual(capacity, area.Capacity);
            Assert.AreEqual(targetResourceType, area.TargetResource);
            Assert.AreEqual(currentCount, area.CurrentCount);
        }

        [Test]
        public void WhenInvalidArgumentsThenException()
        {
            //Assert:
            Assert.Catch<ArgumentException>(() => { _ = new BaseArea(0, new Resource("A"), 5); });
            Assert.Catch<ArgumentException>(() => { _ = new BaseArea(5, new Resource("A"), -3); });
            Assert.Catch<ArgumentException>(() => { _ = new BaseArea(-3, new Resource("A"), 5); });
            Assert.Catch<ArgumentException>(() => { _ = new BaseArea(3, new Resource("A"), -5); });
        }

        [Test]
        public void WhenNullResourceThenException()
        {
            //Assert:
            Assert.Catch<NullReferenceException>(() => { _ = new BaseArea(5, null, 5); });
            Assert.Catch<NullReferenceException>(() => { _ = new BaseArea(3, null, 5); });
            Assert.Catch<NullReferenceException>(() => { _ = new BaseArea(3, null, 3); });
        }

        [TestCaseSource(nameof(CanAddResourcesCases))]
        public bool CanAddResources(BaseArea area, Resource resource, int count)
        {
            return area.CanAddResources(resource, count);
        }

        private static IEnumerable<TestCaseData> CanAddResourcesCases()
        {
            yield return new TestCaseData(
                new BaseArea(capacity: 5, new Resource(name: "Wood"), currentCount: 0),
                new Resource(name: "Wood"),
                3
            ).Returns(true).SetName("Empty area");

            yield return new TestCaseData(
                new BaseArea(capacity: 1, new Resource(name: "Wood"), currentCount: 0),
                new Resource(name: "Wood"),
                2
            ).Returns(false).SetName("Too much");

            yield return new TestCaseData(
                new BaseArea(capacity: 5, new Resource(name: "Wood"), currentCount: 5),
                new Resource(name: "Wood"),
                1
            ).Returns(false).SetName("Is full");

            yield return new TestCaseData(
                new BaseArea(capacity: 5, new Resource(name: "Wood"), currentCount: 0),
                new Resource(name: "Lambert"),
                2
            ).Returns(false).SetName("Not a target resource");
        }

        [TestCaseSource(nameof(CanRemoveResourcesCases))]
        public bool CanRemoveResources(BaseArea area, Resource resource, int count)
        {
            return area.CanRemoveResources(resource, count);
        }

        private static IEnumerable<TestCaseData> CanRemoveResourcesCases()
        {
            yield return new TestCaseData(
                new BaseArea(capacity: 5, new Resource(name: "Wood"), currentCount: 0),
                new Resource(name: "Wood"),
                3
            ).Returns(false).SetName("Empty area");

            yield return new TestCaseData(
                new BaseArea(capacity: 2, new Resource(name: "Wood"), currentCount: 2),
                new Resource(name: "Wood"),
                3
            ).Returns(false).SetName("Too much");

            yield return new TestCaseData(
                new BaseArea(capacity: 5, new Resource(name: "Wood"), currentCount: 5),
                new Resource(name: "Wood"),
                5
            ).Returns(true).SetName("Is full");

            yield return new TestCaseData(
                new BaseArea(capacity: 5, new Resource(name: "Wood"), currentCount: 3),
                new Resource(name: "Lambert"),
                2
            ).Returns(false).SetName("Not a target resource");
        }

        [TestCaseSource(nameof(AddResourcesSuccessfulCases))]
        public void AddResourcesSuccessful(BaseArea area, Resource resource, int count)
        {
            //Arrange:
            Resource addedResource = null;
            int addedCount = 0;
            int previousCount = area.CurrentCount;

            area.OnAdded += (i, p) =>
            {
                addedResource = i;
                addedCount = p;
            };

            //Act:
            bool success = area.AddResources(resource, count);

            //Assert:
            Assert.IsTrue(success);
            Assert.AreEqual(previousCount + count, area.CurrentCount);
            Assert.AreEqual(addedResource, resource);
            Assert.AreEqual(addedCount, count);
        }

        private static IEnumerable<TestCaseData> AddResourcesSuccessfulCases()
        {
            yield return new TestCaseData(
                new BaseArea(capacity: 5, new Resource(name: "Wood"), currentCount: 0),
                new Resource(name: "Wood"),
                3
            ).SetName("Empty area");

            yield return new TestCaseData(
                new BaseArea(capacity: 5, new Resource(name: "Wood"), currentCount: 4),
                new Resource(name: "Wood"),
                1
            ).SetName("Make full 4/5");

            yield return new TestCaseData(
                new BaseArea(capacity: 100, new Resource(name: "Wood"), currentCount: 50),
                new Resource(name: "Wood"),
                50
            ).SetName("Make full 50/100");
        }

        [TestCaseSource(nameof(RemoveResourcesSuccessfulCases))]
        public void RemoveResourcesSuccessful(BaseArea area, Resource resource, int count)
        {
            //Arrange:
            Resource removedResource = null;
            int removedCount = 0;
            int previousCount = area.CurrentCount;

            area.OnRemoved += (i, p) =>
            {
                removedResource = i;
                removedCount = p;
            };

            //Act:
            bool success = area.RemoveResources(resource, count);

            //Assert:
            Assert.IsTrue(success);
            Assert.AreEqual(previousCount - count, area.CurrentCount);
            Assert.AreEqual(removedResource, resource);
            Assert.AreEqual(removedCount, count);
        }

        private static IEnumerable<TestCaseData> RemoveResourcesSuccessfulCases()
        {
            yield return new TestCaseData(
                new BaseArea(capacity: 5, new Resource(name: "Wood"), currentCount: 5),
                new Resource(name: "Wood"),
                5
            ).SetName("Make full empty 5->0");

            yield return new TestCaseData(
                new BaseArea(capacity: 50, new Resource(name: "Wood"), currentCount: 35),
                new Resource(name: "Wood"),
                35
            ).SetName("Make not full empty 35->0");

            yield return new TestCaseData(
                new BaseArea(capacity: 5, new Resource(name: "Wood"), currentCount: 3),
                new Resource(name: "Wood"),
                2
            ).SetName("Take some 3->1");
        }
    }
}