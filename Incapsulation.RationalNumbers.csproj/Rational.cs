using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.RationalNumbers
{
    // Рациональное число - это число, которое можно представить в виде обыкновенной несократимой дроби.
    // Числитель - целое число, а знаменатель - натуральное число. 
    public class Rational 
    {
        public int Numerator { get; }
        public int Denominator { get; }
        public bool IsNan { get; }

        public Rational(int numerator, int denominator = 1)
        {
            var (correctNum, correctDen) = GetCorrectNumAndDen(numerator, denominator);
            Numerator = correctNum;
            Denominator = correctDen;
            IsNan = denominator == 0;
        }

        private static Tuple<int, int> GetCorrectNumAndDen(int num, int den)
        {
            if (num == 0 && den == 0) return new Tuple<int, int>(0, 0);

            var gcd = GetGcd(num, den);
            var correctNum = num / gcd;
            var correctDen = den / gcd;
            
            if (den < 0)
            {
                correctNum *= -1;
                correctDen *= -1;
            }

            return new Tuple<int, int>(correctNum, correctDen);
        }

        // НОД двух чисел
        private static int GetGcd(int x, int y)
        {
            x = Math.Abs(x);
            y = Math.Abs(y);

            if (x == 0) return y;
            if (y == 0) return x;
            if (x == y) return x;
            if (x == 1 || y == 1) return 1;
            
            while (x != y)
            {
                if (x > y) x -= y;
                else y -= x;
            }

            return x;
        }

        public static Rational operator +(Rational r1, Rational r2)
        {
            if (r1.IsNan || r2.IsNan) return new Rational(1, 0);
            
            var newNumerator = r1.Numerator * r2.Denominator + r2.Numerator * r1.Denominator;
            var newDenominator = r1.Denominator * r2.Denominator;
            
            return new Rational(newNumerator, newDenominator);
        }
        
        public static Rational operator -(Rational r1, Rational r2)
        {
            if (r1.IsNan || r2.IsNan) return new Rational(1, 0);
            
            var newNumerator = r1.Numerator * r2.Denominator - r2.Numerator * r1.Denominator;
            var newDenominator = r1.Denominator * r2.Denominator;
            
            return new Rational(newNumerator, newDenominator);
            // вместо всего можно написать одну строчку: return r1 + new Rational((-1) * r2.Numerator, r2.Denominator);
            // но так не будет видно логики вычислений, и лишний раз отработает метод GetCorrectNumAndDen, что долго.
        }
        
        public static Rational operator *(Rational r1, Rational r2)
        {
            if (r1.IsNan || r2.IsNan) return new Rational(1, 0);
            
            var newNumerator = r1.Numerator * r2.Numerator;
            var newDenominator = r1.Denominator * r2.Denominator;
            
            return new Rational(newNumerator, newDenominator);
        }
        
        public static Rational operator /(Rational r1, Rational r2)
        {
            if (r1.IsNan || r2.IsNan) return new Rational(1, 0);

            var newNumerator = r1.Numerator * r2.Denominator;
            var newDenominator = r1.Denominator * r2.Numerator;

            return new Rational(newNumerator, newDenominator);
            // аналогично вычитанию, можно было бы написать: return r1 * new Rational(r2.Denominator, r2.Numerator);
            // но -//-
        }

        public static implicit operator double(Rational rational)
        {
            if (rational.IsNan) return double.NaN;
            
            return (double)rational.Numerator / rational.Denominator;
        }

        public static implicit operator Rational(int n)
        {
            return new Rational(n, 1);
        }
        
        public static explicit operator int(Rational r)
        {
            if (!r.IsNan && (r.Numerator % r.Denominator == 0)) return r.Numerator / r.Denominator;
            else throw new InvalidCastException("Is non convertible to int");
        }
    }
}
