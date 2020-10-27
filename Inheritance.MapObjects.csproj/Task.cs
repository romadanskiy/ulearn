using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.MapObjects
{
    public interface IHaveArmy
    {
        Army Army { get; set; }
    }

    public interface IHaveTreasure
    {
        Treasure Treasure { get; set; }
    }

    public interface IHaveOwner
    {
        int Owner { get; set; }
    }
    
    public class Dwelling : IHaveOwner
    {
        public int Owner { get; set; }
    }

    public class Mine : IHaveArmy, IHaveOwner, IHaveTreasure
    {
        public int Owner { get; set; }
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Creeps : IHaveArmy, IHaveTreasure
    {
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Wolves : IHaveArmy
    {
        public Army Army { get; set; }
    }

    public class ResourcePile : IHaveTreasure
    {
        public Treasure Treasure { get; set; }
    }

    public static class Interaction
    {
        public static void Make(Player player, object mapObject)
        {
            if (mapObject is IHaveArmy objWithArmy)
            {
                if (player.CanBeat(objWithArmy.Army))
                    TryAssignObjectAndCollectTreasure(objWithArmy, player);
                else
                    player.Die();
            }
            else
                TryAssignObjectAndCollectTreasure(mapObject, player);
        }

        private static void TryAssignObjectAndCollectTreasure(object obj, Player player)
        {
            if (obj is IHaveOwner objWithOwner)
                objWithOwner.Owner = player.Id;
            if (obj is IHaveTreasure objWithTreasure)
                player.Consume(objWithTreasure.Treasure);
        }
    }
}