using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YieldTest
{
    public class YieldDemo
    {
        public int GeneratedRandoms { get; private set; }

        Random _r = new Random();

        public IEnumerable<object> EmptyCollection()
        {
            yield break;
        }

        public IEnumerable<int> RandomGenerator()
        {
            GeneratedRandoms++;
            yield return _r.Next();
        }

        public IEnumerable<int> LimitedRandomGenerator()
        {
            for(int i=0;i<10;i++)
            {
                GeneratedRandoms++;
                yield return _r.Next();
            }
        }

        public IEnumerable<int> InfiniteLoopRandomGenerator()
        {
            while(true)
            {
                GeneratedRandoms++;
                yield return _r.Next();
            }
        }

        public IEnumerable<int> OnlyTwoNumbers()
        {
            yield return 1;
            yield return 2;
        }


        public IEnumerable<int> NConsecutiveNumbers(int count)
        {
            int i = 0;
            while (i < count)
            {
                yield return i++;
            }
        }

        public IEnumerable<string> EnumerableStrings()
        {
            yield return "first";
            for (int i = 0; i < 10; i++)
            {
                yield return "element "+i.ToString();
            }
            yield return "last";
        }


        public IEnumerable<string> CycleStrings(string[] values)
        {
            if (values == null || values.Length == 0)
                yield break;

            var enumerator = values.AsEnumerable().GetEnumerator();
            while (true)
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
                enumerator.Reset();
            }
        }

        public IEnumerable<int> AllPositiveInts()
        {
            int value = 0;
            while (value < int.MaxValue)
            {
                yield return value++;
            }
        }
    }

    public class CustomEnumerableDemo : IEnumerable<int>
    {

        public IEnumerator<int> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public class CustomEnumeratorDemo : IEnumerator<int>
    {

        public int Current
        {
            get { throw new NotImplementedException(); }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        object System.Collections.IEnumerator.Current
        {
            get { throw new NotImplementedException(); }
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
