using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.PairsAnalysis
{
    public static class Analysis
    {
        public static int FindMaxPeriodIndex(params DateTime[] data)
        {
            return data.Pairs().Select(tuple => (tuple.Item2 - tuple.Item1).TotalSeconds).MaxIndex();
        }

        public static double FindAverageRelativeDifference(params double[] data)
        {
            return data.Pairs().Select(tuple => (tuple.Item2 - tuple.Item1) / tuple.Item1).Average();
        }
    }

    public static class EnumerableExtension
    {
        public static IEnumerable<Tuple<T, T>> Pairs<T>(this IEnumerable<T> enumerable)
        {
            /*
            var enumerator = enumerable.GetEnumerator();
            
            if (!enumerator.MoveNext())
                throw new ArgumentException();
            var prev = enumerator.Current;
            
            if (!enumerator.MoveNext())
                throw new ArgumentException();
            var cur = enumerator.Current;
            
            yield return Tuple.Create(prev, cur);
            
            while (enumerator.MoveNext())
            {
                prev = cur;
                cur = enumerator.Current;
                yield return Tuple.Create(prev, cur);
            }
            */

            var counter = 0;
            var prev = default(T);
            foreach (var cur in enumerable)
            {
                if (counter == 0)
                {
                    prev = cur;
                    counter++;
                    continue;
                }
                
                yield return Tuple.Create(prev, cur);
                prev = cur;
                counter++;
            }
            
            if (counter == 0) 
                throw new ArgumentException();
        }
        
        public static int MaxIndex<T>(this IEnumerable<T> enumerable) where T: IComparable
        {
            /*
            var enumerator = enumerable.GetEnumerator();
            
            if (!enumerator.MoveNext())
                throw new ArgumentException();

            var max = enumerator.Current;
            var index = 0;
            var maxIndex = 0;
            while (enumerator.MoveNext())
            {
                index++;
                if (enumerator.Current.CompareTo(max) > 0)
                {
                    maxIndex = index;
                }
            }
            
            return maxIndex;
            */
            
            var counter = 0;
            var max = default(T);
            var maxIndex = 0;
            foreach (var cur in enumerable)
            {
                if (counter == 0)
                {
                    max = cur;
                    counter++;
                    continue;
                }

                if (cur.CompareTo(max) > 0)
                {
                    max = cur;
                    maxIndex = counter;
                }

                counter++;
            }
            
            if (counter == 0) 
                throw new ArgumentException();

            return maxIndex;
        }
    }
}
