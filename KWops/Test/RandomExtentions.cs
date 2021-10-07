using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    public static class RandomExtentions
    {
        public static int NextPositive(this Random random, int exclusiveMaximum = int.MaxValue - 1000)
        {
            return random.Next(1, exclusiveMaximum);
        }

        public static int NextZeroOrNegative(this Random random, int exclusiveMinimum = int.MinValue + 1000)
        {
            return -1 * random.Next(0, -1 * exclusiveMinimum);
        }

        public static bool NextBool(this Random random)
        {
            int zeroOrOne = random.Next(0, 2);
            return zeroOrOne == 1;
        }

        public static string NextString(this Random random)
        {
            return Guid.NewGuid().ToString();
        }

        public static DateTime NextDateTimeInFuture(this Random random)
        {
            return DateTime.Now.AddDays(random.Next(1, 3651));
        }
    }
}
