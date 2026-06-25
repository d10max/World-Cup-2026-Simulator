using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace World_Cup_2026_Simulator.Helpers
{
    public static class Helper
    {
        public static double Factorial(int n)
        {
            if (n == 0) return 1.0;

            double result = 1.0;
            for (int i = 1; i <= n; i++)
            {
                result *= i;
            }

            return result;
        }
    }
}
