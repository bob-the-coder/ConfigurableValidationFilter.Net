using System;
using System.Diagnostics;
using BoxFilterExample;
using FluentAssertions;
using NUnit.Framework;

namespace BoxFilter.Test
{
    public class Tests
    {
        private ConfigurableBoxFilter _sut;
        private BoxBuilder _boxBuilder;
        private static readonly Random Rng = new ();

        [SetUp]
        public void Setup()
        {
            _boxBuilder = new BoxBuilder();

            _sut = ConfigurableBoxFilter.Default();
        }

        [Test]
        public void ServiceShouldBeAbleToApplyFilterToBox()
        {
            var box1 = _boxBuilder.Heavy().Old().Tall().Thin().Colored("Black").Build();

            var watch = Stopwatch.StartNew();
            var result = _sut.ApplyConfiguration(box1, BoxFilters.TallAndHeavy);
            watch.Stop();

            result.Count.Should().Be(0, "The box is Tall and Heavy");
            Console.WriteLine($"Time elapsed: {watch.ElapsedMilliseconds}ms");
            Console.WriteLine(string.Join("\n", result));

            watch = Stopwatch.StartNew();
            result = _sut.ApplyConfiguration(box1, BoxFilters.SmallAndLight);
            watch.Stop();

            result.Count.Should().BeGreaterThan(0, "The box is neither Small nor Light");
            Console.WriteLine($"Time elapsed: {watch.ElapsedMilliseconds}ms");
            Console.WriteLine(string.Join("\n", result));
        }

        [TestCase(1)]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        [TestCase(100000)]
        [TestCase(1000000)]
        [TestCase(10000000)]
        public void PerformanceMetrics(int nBoxes)
        {
            int totalSucceeded = 0,
                totalFailed = 0;

            double totalElapsedSucceeded = 0,
                totalElapsedFailed = 0;

            while (nBoxes-- > 0)
            {
                var (success, elapsedMs) = CheckOneBox();

                if (success)
                {
                    totalSucceeded++;
                    totalElapsedSucceeded += elapsedMs;
                }
                else
                {
                    totalFailed++;
                    totalElapsedFailed += elapsedMs;
                }

                //Console.WriteLine(Roll);
            }

            Console.WriteLine($"Boxes checked: {totalFailed + totalSucceeded}");
            Console.WriteLine($"Total duration: {TimeSpan.FromMilliseconds(totalElapsedFailed + totalElapsedSucceeded)}");
            Console.WriteLine();
            Console.WriteLine($"Total failed: {totalFailed}");
            Console.WriteLine($"Total duration (failed): {TimeSpan.FromMilliseconds(totalElapsedFailed)}");
            Console.WriteLine($"Average duration (failed): {TimeSpan.FromMilliseconds(totalFailed == 0 ? 0 : totalElapsedFailed / totalFailed)}");
            Console.WriteLine();
            Console.WriteLine($"Total succeeded: {totalSucceeded}");
            Console.WriteLine($"Total duration (succeeded): {TimeSpan.FromMilliseconds(totalElapsedSucceeded)}");
            Console.WriteLine($"Average duration (succeeded): {TimeSpan.FromMilliseconds(totalSucceeded == 0 ? 0 : totalElapsedSucceeded / totalSucceeded)}");
        }

        private (bool success, double elapsedMs) CheckOneBox()
        {
            var box = BoxBuilder.RandomBox();

            var watch = Stopwatch.StartNew();
            var result = _sut.ApplyConfiguration(box, BoxFilters.PlainBrownBox);
            watch.Stop();

            return (result.Count == 0, watch.Elapsed.TotalMilliseconds);
        }
    }
}