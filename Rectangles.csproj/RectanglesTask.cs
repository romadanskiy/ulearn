using System;

namespace Rectangles
{
	public static class RectanglesTask
	{
		// Пересекаются ли два прямоугольника (пересечение только по границе также считается пересечением)
		public static bool AreIntersected(Rectangle r1, Rectangle r2)
		{
            return !(r1.Top > r2.Bottom || r1.Bottom < r2.Top || r1.Left > r2.Right || r1.Right < r2.Left);
        }

        // Площадь пересечения прямоугольников
        public static int IntersectionSquare(Rectangle r1, Rectangle r2)
		{
            if (AreIntersected(r1, r2))
            {
                // Координаты получившегося прямоугольника
                var top = Math.Max(r1.Top, r2.Top);
                var bottom = Math.Min(r1.Bottom, r2.Bottom);
                var left = Math.Max(r1.Left, r2.Left);
                var right = Math.Min(r1.Right, r2.Right);

                return (bottom - top) * (right - left);
            }
			else return 0;
		}

		// Если один из прямоугольников целиком находится внутри другого — вернуть номер (с нуля) внутреннего.
		// Иначе вернуть -1
		// Если прямоугольники совпадают, можно вернуть номер любого из них.
		public static int IndexOfInnerRectangle(Rectangle r1, Rectangle r2)
		{
            /*var square1 = r1.Width * r1.Height; // Площадь первого прямоугольника
            var square2 = r2.Width * r2.Height; // Площадь второго прямоугольника
            if (AreIntersected(r1, r2) && (Math.Max(r1.Right, r2.Right) - Math.Min(r1.Left, r2.Left) <= Math.Max(r1.Width, r2.Width)) && (Math.Max(r1.Bottom, r2.Bottom) - Math.Min(r1.Top, r2.Top) <= Math.Max(r1.Height, r2.Height)))
            {
                if ((IntersectionSquare(r1, r2) == square1) && (r1.Width <= r2.Width) && (r1.Height <= r2.Width)) return 0;
                else if ((IntersectionSquare(r1, r2) == square2) && (r2.Width <= r1.Width) && (r2.Height <= r1.Width)) return 1;
                else return -1;
            }
            else return -1;*/

            if (r1.Left >= r2.Left && r1.Top >= r2.Top && r1.Right <= r2.Right && r1.Bottom <= r2.Bottom) return 0;
            else if (r1.Left <= r2.Left && r1.Top <= r2.Top && r1.Right >= r2.Right && r1.Bottom >= r2.Bottom) return 1;
            else return -1;
		}
	}
}