using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var n = 3;
            Console.WriteLine(Fibonacci.GetNumber(n));
        }


    }

    public static class Fibonacci
    {
        private static readonly Dictionary<int, BigInteger[][]> Memo = new Dictionary<int, BigInteger[][]>();
        private static readonly BigInteger[][] Matrix = {
            new BigInteger[] {1, 1},
            new BigInteger[] {1, 0}
        };

        private static BigInteger[][] MultMatrices(BigInteger[][] matrix1, BigInteger[][] matrix2)
        {
            var a11 = matrix1[0][0] * matrix2[0][0] + matrix1[0][1] * matrix2[1][0];
            var a12 = matrix1[0][0] * matrix2[0][1] + matrix1[0][1] * matrix2[1][1];
            var a21 = matrix1[1][0] * matrix2[0][0] + matrix1[1][1] * matrix2[1][0];
            var a22 = matrix1[1][0] * matrix2[0][1] + matrix1[1][1] * matrix2[1][1];
            return new []
            {
                new[] {a11, a12},
                new[] {a21, a22}
            };
        }

        private static BigInteger[][] PowMatrix(BigInteger[][] matrix, int power)
        {
            if (power == 1)
                return matrix;
            Debug.Assert(power % 2 == 0);
            if (Memo.TryGetValue(power, out var memorized))
                return memorized;
            var k = PowMatrix(matrix, power / 2);
            var r = MultMatrices(k, k);
            Memo.Add(power, r);
            return r;
        }

        public static BigInteger GetNumber(int n)
        {
            switch (n)
            {
                case 0:
                    return 0;
                case 1:
                    return 1;
            }

            var powers = new List<int>();
            var binaryFormatted = Convert.ToString(n, 2);
            for (var i = 0; i < binaryFormatted.Length ; i++)
            {
                var binary = binaryFormatted[binaryFormatted.Length - i -1];
                if(binary == '1')
                    powers.Add((int) Math.Pow(2, i));
            }


            var matrices = powers.Select(power => PowMatrix(Matrix, power)).ToList();

            while (matrices.Count > 1)
            {
                var m1 = matrices[0];
                var m2 = matrices[1];
                matrices.Remove(m1);
                matrices.Remove(m2);
                var r = MultMatrices(m1, m2);
                matrices.Add(r);
            }

            return matrices[0][0][0];
        }
    }

}
