using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

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
            ConverterArea loadingArea = new ConverterArea(5);
            ConverterArea unloadingArea = new ConverterArea(5);

            //Act:
            Converter<string> converter = new Converter<string>(loadingArea, unloadingArea, inCountToConvert, outCountAfterConvert,
                timeToConvert,
                "Wood",
                "Lambert");

            //Assert:
            Assert.AreEqual(loadingArea, converter.LoadingArea);
            Assert.AreEqual(unloadingArea, converter.UnloadingArea);
            Assert.AreEqual(inCountToConvert, converter.InCountToConvert);
            Assert.AreEqual(outCountAfterConvert, converter.OutCountAfterConvert);
            Assert.AreEqual(timeToConvert, converter.TimeToConvert);
        }

        [TestCase(-4, 1, 1f)]
        [TestCase(1, -2, 1f)]
        [TestCase(1, 1, 0f)]
        public void WhenInvalidArgumentsThenException(int inCount, int outCount, float time)
        {
            ConverterArea loadingArea = new ConverterArea(5);
            ConverterArea unloadingArea = new ConverterArea(5);

            //Assert:
            Assert.Catch<ArgumentException>(() =>
            {
                _ = new Converter<string>(loadingArea, unloadingArea, inCount, outCount, time,
                    "Wood",
                    "Lambert");
            });

            Assert.Catch<NullReferenceException>(() =>
            {
                _ = new Converter<string>(null, unloadingArea, 1, 1, 0f,
                    "Wood",
                    "Lambert");
            });
            Assert.Catch<NullReferenceException>(() =>
            {
                _ = new Converter<string>(loadingArea, null, 1, 1, 0f,
                    "Wood",
                    "Lambert");
            });
        }

        [Test]
        public void WhenNullArgumentsThenException()
        {
            Assert.Catch<NullReferenceException>(() =>
            {
                _ = new Converter<string>(null, new ConverterArea(5), 1, 1, 0f,
                    "Wood",
                    "Lambert");
            });
            Assert.Catch<NullReferenceException>(() =>
            {
                _ = new Converter<string>(new ConverterArea(5), null, 1, 1, 0f,
                    "Wood",
                    "Lambert");
            });
        }

        [TestCaseSource(nameof(CanPutResourcesCases))]
        public bool CanPutResources(Converter<string> converter, string resource, int count)
        {
            return converter.CanPutResources(resource, count);
        }

        private static IEnumerable<TestCaseData> CanPutResourcesCases()
        {
            yield return new TestCaseData(
                new Converter<string>(new ConverterArea(5), new ConverterArea(5), 2, 1, 1f,
                    "Wood",
                    "Lambert"),
               "Wood",
                3
            ).Returns(true).SetName("Empty area");

            yield return new TestCaseData(
                new Converter<string>(new ConverterArea(5), new ConverterArea(5), 2, 1, 1f,
                    "Wood",
                    "Lambert"),
               "Wood",
                6
            ).Returns(true).SetName("Too much");

            yield return new TestCaseData(
                new Converter<string>(new ConverterArea(5), new ConverterArea(5), 2, 1, 1f,
                    "Wood",
                    "Lambert"),
                 "Lambert",
                2
            ).Returns(false).SetName("Not a target resource");
        }

        [TestCaseSource(nameof(CanTakeResourcesCases))]
        public bool CanTakeResources(Converter<string> converter, string resource, int count)
        {
            return converter.CanTakeResources(resource, count);
        }

        private static IEnumerable<TestCaseData> CanTakeResourcesCases()
        {
            yield return new TestCaseData(
                new Converter<string>(new ConverterArea(5), new ConverterArea(5), 2, 1, 1f,
                    "Wood",
                    "Lambert"),
                "Lambert",
                3
            ).Returns(false).SetName("Empty area");

            yield return new TestCaseData(
                new Converter<string>(new ConverterArea(5), new ConverterArea(5), 2, 1, 1f,
                    "Wood",
                    "Lambert"),
                "Lambert",
                6
            ).Returns(false).SetName("Too much");

            yield return new TestCaseData(
                new Converter<string>(new ConverterArea(5), new ConverterArea(5, 5), 2, 1, 1f,
                   "Wood",
                    "Lambert"),
               "Lambert",
                5
            ).Returns(true).SetName("Is full");

            yield return new TestCaseData(
                new Converter<string>(new ConverterArea(5), new ConverterArea(5, 3), 2, 1, 1f,
                     "Wood",
                    "Lambert"),
                "Lambert",
                2
            ).Returns(true).SetName("Success");

            yield return new TestCaseData(
                new Converter<string>(new ConverterArea(5), new ConverterArea(5), 2, 1, 1f,
                    "Wood",
                    "Lambert"),
                "Wood",
                2
            ).Returns(false).SetName("Not a target resource");
        }

        [TestCaseSource(nameof(PutResourcesSuccessfulCases))]
        public void PutResourcesSuccessful(Converter<string> converter, string resource, int count)
        {
            //Arrange:
            string puttedResource = null;
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
                new Converter<string>(new ConverterArea(5), new ConverterArea(5, 3), 2, 1, 1f,
                     "Wood",
                   "Lambert"),
                "Wood",
                3
            ).SetName("Empty area");

            yield return new TestCaseData(
                new Converter<string>(new ConverterArea(5, 4), new ConverterArea(5, 3), 2, 1, 1f,
                     "Wood",
                     "Lambert"),
                "Wood",
                1
            ).SetName("Make full 4/5");

            yield return new TestCaseData(
                new Converter<string>(new ConverterArea(100, 50), new ConverterArea(5, 3), 2, 1, 1f,
                    "Wood",
                     "Lambert"),
                "Wood",
                50
            ).SetName("Make full 50/100");
        }

        [TestCaseSource(nameof(TakeResourcesSuccessfulCases))]
        public void TakeResourcesSuccessful(Converter<string> converter, string resource, int count)
        {
            //Arrange:
            string takenResource = null;
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
                new Converter<string>(new ConverterArea(5), new ConverterArea(5, 3), 2, 1, 1f,
                    "Wood",
                    "Lambert"),
               "Lambert",
                3
            ).SetName("Take all");

            yield return new TestCaseData(
                new Converter<string>(new ConverterArea(5, 4), new ConverterArea(5, 3), 2, 1, 1f,
                    "Wood",
                    "Lambert"),
                "Lambert",
                1
            ).SetName("Take one");

            yield return new TestCaseData(
                new Converter<string>(new ConverterArea(100, 50), new ConverterArea(100, 100), 2, 1, 1f,
                    "Wood",
                    "Lambert"),
                "Lambert",
                50
            ).SetName("Take half 50/100");
        }

        [TestCaseSource(nameof(ConvertResourcesSuccessfulCases))]
        public void ConvertResourcesSuccessful(Converter<string> converter, Resource resource, int count, int loadingAreaCount,
            int unloadingAreaCount, bool successfull)
        {
            //Arrange:
            List<string> convertedResources = new();
            int convertedCount = 0;

            converter.OnConvert += (i, p) =>
            {
                convertedResources.Add(i);
                convertedCount += p;
            };

            //Act:
            bool success = converter.StartConvert();
            for (int i = 0; i < count * 2; i++)
            {
                converter.Update(1f);
                Debug.Log(converter.LoadingArea.CurrentCount);
                Debug.Log(converter.UnloadingArea.CurrentCount);
            }

            //Assert:
            foreach (string convertedResource in convertedResources)
            {
                Assert.AreEqual(resource.Name, convertedResource);
            }

            Assert.AreEqual(success, successfull);
            Assert.AreEqual(count, convertedCount);
            Assert.AreEqual(loadingAreaCount, converter.LoadingArea.CurrentCount);
            Assert.AreEqual(unloadingAreaCount, converter.UnloadingArea.CurrentCount);
        }

        private static IEnumerable<TestCaseData> ConvertResourcesSuccessfulCases()
        {
            yield return new TestCaseData(
                new Converter<string>(new ConverterArea(5, 5), new ConverterArea(5, 0), 2, 1, 1f,
                    "Wood",
                    "Lambert"),
                new Resource(name: "Lambert"),
                2,
                1,
                2,
                true
            ).SetName("Was empty, 5->2, final 2/5");

            yield return new TestCaseData(
                new Converter<string>(new ConverterArea(5, 1), new ConverterArea(5, 0), 2, 1, 1f,
                    "Wood",
                    "Lambert"),
                new Resource(name: "Lambert"),
                0,
                1,
                0,
                false
            ).SetName("Was empty, 1->0");

            yield return new TestCaseData(
                new Converter<string>(new ConverterArea(100, 50), new ConverterArea(10, 10), 2, 1, 1f,
                    "Wood",
                    "Lambert"),
                new Resource(name: "Lambert"),
                0,
                50,
                10,
                false
            ).SetName("Was full, 50->0");

            yield return new TestCaseData(
                new Converter<string>(new ConverterArea(100, 50), new ConverterArea(10, 5), 2, 1, 1f,
                    "Wood",
                    "Lambert"),
                new Resource(name: "Lambert"),
                5,
                40,
                10,
                true
            ).SetName("Was not full, 50->5");
        }
    }
}