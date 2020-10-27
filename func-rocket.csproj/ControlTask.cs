using System;

namespace func_rocket
{
	public class ControlTask
	{
		public static Turn ControlRocket(Rocket rocket, Vector target)
		{
			var v = target - rocket.Location;
			var ang1 = v.Angle - rocket.Velocity.Angle;
			var ang2 = v.Angle - rocket.Direction;
			var a = 0.555;
			var resAng = 0.0;
			if (ang1 < a || ang2 < a) resAng = ang1 + ang2;
			if (ang1 > a || ang2 > a) resAng = ang2;

			var result = Turn.None;
			if (resAng > 0.003) result = Turn.Right;
			if (resAng < -0.003) result = Turn.Left;

			return result;
		}
	}
}