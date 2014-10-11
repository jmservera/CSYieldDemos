using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace YieldTest
{
    public class YieldDemoTests
    {
        YieldDemo _yieldDemo=new YieldDemo();

        [Fact]
        public void ShouldReturnEmptyCollection()
        {
            var coll = _yieldDemo.EmptyCollection();
            Assert.Equal(0, coll.Count());
        }

        [Fact]
        public void QueryingIsLazy()
        {
            IEnumerable<int> coll = _yieldDemo.RandomGenerator();
            Assert.Equal(0, _yieldDemo.GeneratedRandoms);
        }

        [Fact]
        public void ShouldntGenerateWholeCollection()
        {
            IEnumerable<int> coll = _yieldDemo.RandomGenerator();
            var value = coll.First();

            Assert.Equal(1, _yieldDemo.GeneratedRandoms);
        }

        [Fact]
        public void CountIsAlwaysOne()
        {
            IEnumerable<int> coll = _yieldDemo.RandomGenerator();
            Assert.Equal(1, coll.Count());
            var enumerator = coll.GetEnumerator();
            Assert.DoesNotThrow(() =>
            {
                enumerator.MoveNext();
                enumerator.MoveNext();
                enumerator.MoveNext();
                enumerator.MoveNext();
                var current = enumerator.Current;
            });
            enumerator.MoveNext();
            Assert.Equal(1, coll.Count());
        }

        [Fact]
        public void CountIsNWhenLimited()
        {
            IEnumerable<int> coll = _yieldDemo.LimitedRandomGenerator();

            Assert.Equal(10, coll.Count());
            foreach (var value in coll)
            {
                Assert.True(value >= 0);
            }
        }

        [Fact]
        public void SkipWorksWhenInLimitedLoop()
        {
            IEnumerable<int> coll = _yieldDemo.LimitedRandomGenerator();
            var value = coll.Skip(5).First();
            Assert.True(value >= 0);
        }

        [Fact]
        public void CustomEnumerationDoesNotSupportSkip()
        {
            var coll = _yieldDemo.RandomGenerator();

            Assert.Throws<InvalidOperationException>(() =>
            {
                var intermediate = coll.Skip(5);
                var value=intermediate.First();
                Assert.Equal(6, _yieldDemo.GeneratedRandoms);
            });
        }

        [Fact]
        public void ShouldReturnTwoConsecutiveNumbers()
        {
            IEnumerable<int> coll = _yieldDemo.OnlyTwoNumbers();
            var counter = 0;
            foreach (var i in coll)
            {
                Assert.Equal(i, ++counter);
            }

            Assert.Equal(2, counter);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(50)]
        [InlineData(100)]
        public void ShouldReturnAnyConsecutiveNumbers(int count)
        {
            IEnumerable<int> coll = _yieldDemo.NConsecutiveNumbers(count);
            var counter=0;
            foreach (var i in coll)
            {
                Assert.Equal(counter++,i);
            }
            Assert.Equal(count, counter);
        }

        [Fact]
        public void ConcreteCollectionSupportsSkip()
        {
            IEnumerable<int> coll = _yieldDemo.NConsecutiveNumbers(50);
            var ten = coll.Skip(10).First();
            Assert.Equal(10, ten);
        }

        [Fact]
        public void IteratorStartsAndEnds()
        {
            IEnumerable<string> coll = _yieldDemo.EnumerableStrings();
            int i = 0;
            foreach (var s in coll)
            {
                if (i++ == 0)
                {
                    Assert.Equal("first", s);
                }
                else if(i==coll.Count())
                {
                    Assert.Equal("last", s);
                }
                else
                {
                    Assert.StartsWith("element", s);
                }
            }
        }

        [Fact]
        public void IteratorCanCycle()
        {
            IEnumerable<string> coll = _yieldDemo.CycleStrings(new string[] { "Zero", "One" });
            int counter = 0;
            foreach (var value in coll)
            {
                var mod = counter++ % 2;

                if (mod == 0)
                    Assert.Equal("Zero", value);
                else
                    Assert.Equal("One", value);

                if (counter > 10)
                    break; //avoid infinite cycle
            }
            Assert.Equal(11, counter);
            //Assert.Equal(1,coll.Count()); //will create an endless loop
        }
        [Fact]
        public void LoopedIteratorCanUseSkipEvenIfItIsInfinite()
        {
            IEnumerable<int> coll = _yieldDemo.InfiniteLoopRandomGenerator();

            var value = coll.Skip(100).First();
            Assert.True(value >= 0);
            Assert.Equal(101, _yieldDemo.GeneratedRandoms);

        }

        [Fact]
        public void LimitedLoopCountIteratesCount()
        {
            IEnumerable<int> coll = _yieldDemo.AllPositiveInts();
            Assert.Equal(int.MaxValue, coll.Count());
        }

        [Fact]
        public void ParallelForEachWorksWithYield()
        {
            int count = 1000;
            IEnumerable<int> coll = _yieldDemo.NConsecutiveNumbers(count);
            int value = 0;
            int counter = 0;
            Parallel.ForEach(coll, (i) =>
            {
                if (value < i)
                {
                    System.Threading.Interlocked.Exchange(ref value, i);
                }
                System.Threading.Interlocked.Increment(ref counter);
            });
            Assert.True(count - value < 2);
            Assert.Equal(count, counter);
        }

    }
}
