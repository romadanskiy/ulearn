using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digger
{
    //Напишите здесь классы Player, Terrain и другие.
    public class Terrain : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return true;
        }

        public int GetDrawingPriority()
        {
            return 2;
        }

        public string GetImageFileName()
        {
            return "Terrain.png";
        }
    }

    public class Player : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            var command = new CreatureCommand();
            var key = Game.KeyPressed;
            if (key == Keys.Up && y - 1 >= 0 && !(Game.Map[x, y - 1] is Sack))
            {
                command.DeltaY = -1;
            }
            if (key == Keys.Down && y + 1 < Game.MapHeight && !(Game.Map[x, y + 1] is Sack))
            {
                command.DeltaY = 1;
            }
            if (key == Keys.Left && x - 1 >= 0 && !(Game.Map[x - 1, y] is Sack))
            {
                command.DeltaX = -1;
            }
            if (key == Keys.Right && x + 1 < Game.MapWidth && !(Game.Map[x + 1, y] is Sack))
            {
                command.DeltaX = 1;
            }
            
            return command;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Sack || conflictedObject is Monster;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public string GetImageFileName()
        {
            return "Digger.png";
        }
    }

    public class Sack : ICreature
    {
        public int counter;

        public CreatureCommand Act(int x, int y)
        {
            var command = new CreatureCommand() { DeltaX = 0 };
            if (y + 1 < Game.MapHeight &&
                (Game.Map[x, y + 1] is null ||
                (counter > 0 && (Game.Map[x, y + 1] is Player || Game.Map[x, y + 1] is Monster))))
            {
                command.DeltaY = 1;
                counter++;

                return command;
            }
            if (counter > 1 || y == Game.MapHeight)
            {
                command.TransformTo = new Gold();

                return command;
            }
            else counter = 0;

            return new CreatureCommand();

        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return false;
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public string GetImageFileName()
        {
            return "Sack.png";
        }
    }

    public class Gold : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Player)
            {
                Game.Scores += 10;  
            }

            return true;
        }

        public int GetDrawingPriority()
        {
            return 2;
        }

        public string GetImageFileName()
        {
            return "Gold.png";
        }
    }

    public class Positon
    {
        public int X;
        public int Y;
    }

    public class Monster : ICreature
    {
        public Positon FindDiggerPosition()
        {
            for (int x = 0; x < Game.MapWidth; x++)
                for (int y = 0; y < Game.MapHeight; y++)
                {
                    if (Game.Map[x, y] is Player) return new Positon() { X = x, Y = y };
                }

            return null;
        }

        public bool MonsterCanGoTo (int x, int y)
        {
            return Game.Map[x, y] == null || Game.Map[x, y] is Gold || Game.Map[x, y] is Player;
        }
        public CreatureCommand Act(int x, int y)
        {
            var command = new CreatureCommand();
            var diggerPosition = FindDiggerPosition();
            if (diggerPosition != null)
            {
                if (diggerPosition.X < x && MonsterCanGoTo(x - 1, y)) 
                    command.DeltaX = -1;
                if (diggerPosition.X > x && MonsterCanGoTo(x + 1, y))
                    command.DeltaX = 1;
                if (diggerPosition.Y < y && MonsterCanGoTo(x, y - 1)) 
                    command.DeltaY = -1;
                if (diggerPosition.Y > y && MonsterCanGoTo(x, y + 1)) 
                    command.DeltaY = 1;
            }

            return command;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Sack || conflictedObject is Monster;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public string GetImageFileName()
        {
            return "Monster.png";
        }
    }
}
