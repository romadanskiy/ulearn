using System;
using System.Collections.Generic;

namespace func_rocket
{
	public class LevelsTask
	{
		static readonly Physics standardPhysics = new Physics();

		public static IEnumerable<Level> CreateLevels()
		{
			var rocket = new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI);
			var target = new Vector(600, 200);
			yield return MakeLevel("Zero", rocket, target, (size, v) => Vector.Zero);
			yield return MakeLevel("Heavy", rocket, target, (size, v) => new Vector(0, 0.9));
			yield return MakeLevel("Up", rocket, new Vector(700, 500),
				(size, v) =>
				{
					var d = size.Height - v.Y;
					return new Vector(0, -1) * (300 / (d + 300.0));
				});
			yield return MakeLevel("WhiteHole", rocket, target, (size, v) => WhiteHoleGravity(v, target));
			yield return MakeLevel("BlackHole", rocket, target, (size, v) => BlackHoleGravity(v, target, rocket));
			yield return MakeLevel("BlackAndWhite", rocket, target,
				(size, v) =>
				{
					var white = WhiteHoleGravity(v, target);
					var black = BlackHoleGravity(v, target, rocket);
					return (white + black) / 2;
				});
		}

		public static Vector WhiteHoleGravity(Vector v, Vector target)
		{
			var d = Distance(v, target);
			var white = (v - target).Normalize() * (140 * d / (d * d + 1));
			return white;
		}

		public static Vector BlackHoleGravity(Vector v, Vector target, Rocket rocket)
		{
			var anomaly = (rocket.Location + target) / 2;
			var d = Distance(v, anomaly);
			var black = (anomaly - v).Normalize() * (300 * d / (d * d + 1));
			return black;
		}

		public static double Distance(Vector v1, Vector v2)
		{
			return Math.Sqrt((v1.X - v2.X) * (v1.X - v2.X) + (v1.Y - v2.Y) * (v1.Y - v2.Y));
		}

		public static Level MakeLevel(string name, Rocket rocket, Vector target, Gravity gravity)
		{
			return new Level(name, rocket, target, gravity, standardPhysics);
		}
	}
}