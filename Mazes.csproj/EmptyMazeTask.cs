namespace Mazes
{
	public static class EmptyMazeTask
	{
		public static void MoveOut(Robot robot, int width, int height)
		{
            MoveToDirection(robot, Direction.Right, width);
            MoveToDirection(robot, Direction.Down, height);
        }

        public static void MoveToDirection(Robot robot, Direction direction, int distance)
        {
            for (int i = 1; i < distance - 2; i++)
                robot.MoveTo(direction);
        }
	}
}