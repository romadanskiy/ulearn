using System;

namespace DistanceTask
{
	public static class DistanceTask
	{
		// Расстояние от точки (x, y) до отрезка AB с координатами A(ax, ay), B(bx, by)
        // Пусть точка С(x, y)
		public static double GetDistanceToSegment(double ax, double ay, double bx, double by, double x, double y)
		{  
            var AB = Math.Sqrt((ax - bx) * (ax - bx) + (ay - by) * (ay - by)); // Длина отрезка AB
            var BC = Math.Sqrt((bx - x) * (bx - x) + (by - y) * (by - y)); // Длина отрезка BC
            var AC = Math.Sqrt((ax - x) * (ax - x) + (ay - y) * (ay - y)); // Длина отрезка AC
            var p = (AB + BC + AC) / 2; // Полупериметр треугольника ABC
            var h = 2 * Math.Sqrt(p * (p - AB) * (p - BC) * (p - AC)) / AB; // Расстояние от точки С до ПРЯМОЙ AB
            if (AB == 0) return AC; // Если отрезок AB является точкой...
            else if (Cos(BC, AC, AB) < 0) return AC; // Если угол BAC > 90
            else if (Cos(AC, BC, AB) < 0) return BC; // Если угол ABC > 90
            else return h;
		}

        // Коосинус угла треугольника, лежащий против стороны a, между сторонами b и с 
        static double Cos(double a, double b, double c)
        {
            return (b * b + c * c - a * a) / (2 * b * c);
        }
	}
}