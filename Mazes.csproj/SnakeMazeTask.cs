namespace Mazes
{
	public static class SnakeMazeTask
	{
		public static void MoveOut(Robot robot, int width, int height)
		{
            while (true)
            {
                MoveToRight(robot, width);
                if (robot.Finished) break;
                MoveToDown(robot);
                MoveToLeft(robot, width);
                if (robot.Finished) break;
                MoveToDown(robot);
            }
		}

        public static void MoveToRight(Robot robot, int width)
        {
            for (int i = 1; i < width - 2; i++)
                robot.MoveTo(Direction.Right);
        }

        public static void MoveToLeft(Robot robot, int width)
        {
            for (int i = width - 2; i > 1; i--)
                robot.MoveTo(Direction.Left);
        }

        public static void MoveToDown(Robot robot)
        {
            robot.MoveTo(Direction.Down);
            robot.MoveTo(Direction.Down);
        }
    }
}