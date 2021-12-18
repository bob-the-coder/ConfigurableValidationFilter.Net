using System;
using System.Diagnostics;
using BoxFilterExample;
using FluentAssertions;
using NUnit.Framework;

namespace BoxFilterPOC.Tests
{
    public class Tests
    {
        private BoxFilter _sut;
        private BoxBuilder _boxBuilder;

        [SetUp]
        public void Setup()
        {
            _boxBuilder = new BoxBuilder();

            _sut = BoxFilter.Default();
        }

        [Test]
        public void ServiceShouldBeAbleToApplyFilterToBox()
        {
            var box1 = _boxBuilder.Heavy().Old().Tall().Thin().Colored("Black").Build();

            var watch = Stopwatch.StartNew();
            var result = _sut.ApplyConfiguration(box1, BoxFilterConfigurations.TallAndHeavy);
            watch.Stop();

            result.Success.Should().BeTrue("The box is Tall and Heavy");
            Console.WriteLine($"Time elapsed: {watch.ElapsedMilliseconds}ms");
            Console.WriteLine(string.Join("\n", result));

            watch = Stopwatch.StartNew();
            result = _sut.ApplyConfiguration(box1, BoxFilterConfigurations.SmallAndLight);
            watch.Stop();

            result.Success.Should().BeFalse("The box is neither Small nor Light");
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

            var totalElapsedSucceeded = TimeSpan.Zero;
            var totalElapsedFailed = TimeSpan.Zero;

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
            Console.WriteLine($"Total duration: {totalElapsedFailed + totalElapsedSucceeded}");
            Console.WriteLine();
            Console.WriteLine($"Total failed: {totalFailed}");
            Console.WriteLine($"Total duration (failed): {totalElapsedFailed}");
            Console.WriteLine($"Average duration (failed): {TimeSpan.FromMilliseconds(totalFailed == 0 ? 0 : (double)totalElapsedFailed.Milliseconds / totalFailed)}");
            Console.WriteLine();
            Console.WriteLine($"Total succeeded: {totalSucceeded}");
            Console.WriteLine($"Total duration (succeeded): {totalElapsedSucceeded}");
            Console.WriteLine($"Average duration (succeeded): {TimeSpan.FromMilliseconds(totalSucceeded == 0 ? 0 : (double)totalElapsedSucceeded.Milliseconds / totalSucceeded)}");
        }

        private (bool success, TimeSpan elapsed) CheckOneBox()
        {
            var box = BoxBuilder.RandomBox();

            var watch = Stopwatch.StartNew();
            var result = _sut.ApplyConfiguration(box, BoxFilterConfigurations.PlainBrownBox);
            watch.Stop();

            return (result.Success, watch.Elapsed);
        }
    }
}