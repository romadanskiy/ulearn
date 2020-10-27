namespace Mazes
{
	public static class DiagonalMazeTask
	{
		public static void MoveOut(Robot robot, int width, int height)
		{
            if (width > height)
                Move(robot, Direction.Right, Direction.Down, width, height);
            else
                Move(robot, Direction.Down, Direction.Right, height, width);
        }

        public static void Move(Robot robot, Direction direction1, Direction direction2, int maxLength, int minLength)
        {
            while (true)
            {
                MoveToDirection(robot, direction1, maxLength, minLength);
                if (robot.Finished) break;
                robot.MoveTo(direction2);
            }
        }

        public static void MoveToDirection(Robot robot, Direction direction, int a, int b)
        {
            for (int i = 0; i < (a - 2) / (b - 2); i++)
                robot.MoveTo(direction);
        }
    }
}