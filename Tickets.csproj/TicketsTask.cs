using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tickets
{
    public class TicketsTask
    {
        public static BigInteger Solve(int totalLen, int totalSum)
        {
            if (totalSum % 2 != 0) return 0;

            var halfSum = totalSum / 2;
            var arr = new BigInteger[totalLen + 1, halfSum + 1];
            // arr[i, j] равен количеству i-значных чисел, сумма цифр которых равна j
            // исключение arr[0, 0] = 1

            for (var i = 0; i <= totalLen; i++)
                arr[i, 0] = 1;

            for (var i = 1; i <= totalLen; i++)
                for (var j = 1; j <= halfSum; j++)
                    arr[i, j] = GetCount(arr, i - 1, j);

            return arr[totalLen, halfSum] * arr[totalLen, halfSum];
        }

        private static BigInteger GetCount(BigInteger[,] arr, int i, int j)
        {
            var n = 0;
            BigInteger count = 0;
            while (n < 10 && j - n >= 0)
            {
                count += arr[i, j - n];
                n++;
            }

            return count;
        }
    }
}
