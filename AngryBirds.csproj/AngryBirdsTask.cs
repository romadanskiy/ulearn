using System;

namespace AngryBirds
{
	public static class AngryBirdsTask
	{

        /// <param name="v">Начальная скорость</param>
        /// <param name="distance">Расстояние до цели</param>
        /// <returns>Угол прицеливания в радианах от 0 до Pi/2</returns>

        const double g = 9.8;

        public static double FindSightAngle(double v, double distance)
        {
            double angle = Math.Asin((2 * g * distance) / (v * v * 2)) / 2;
            if (angle >= 0 && angle <= Math.PI / 2)
            {
                return angle;
            }
            else
            {
                return double.NaN;
            }
        }
	}
}
