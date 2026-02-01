using UnityEngine;
namespace MagmaLabs.Utilities.Numerics{

public static class Numerics
{
    public static int Fibonacci(int n)
    {
        if (n <= 0) return 0;
        if (n == 1) return 1;
        int a = 0, b = 1, c = 1;
        for (int i = 2; i <= n; i++)
        {
            c = a + b;
            a = b;
            b = c;
        }
        return c;
    }
}
}
