using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rocket_bot
{
    public partial class Bot
    {
        //private Rocket rocket;
        //private Tuple<Turn, double> bestMove;
        //public Rocket GetNextMove(Rocket rocket)
        //{
        //    // TODO: распараллелить запуск SearchBestMove
        //    this.rocket = rocket;
        //    bestMove = Tuple.Create(Turn.None, 0.0);
        //    Parallel.For(0, threadsCount, Search);
        //    var newRocket = rocket.Move(bestMove.Item1, level);
        //    return newRocket;
        //}

        //private void Search(int number)
        //{
        //    var curBestMove = SearchBestMove(rocket, new Random(random.Next()), iterationsCount / threadsCount);
        //    lock (bestMove)
        //    {
        //        if (curBestMove.Item2 > bestMove.Item2)
        //            bestMove = curBestMove;
        //    }
        //}


        public Rocket GetNextMove(Rocket rocket)
        {
            // TODO: распараллелить запуск SearchBestMove
            var bestMove = Tuple.Create(Turn.None, 0.0);
            Parallel.For(0, threadsCount, (i) =>
            {
                var curBestMove = SearchBestMove(rocket, new Random(random.Next()), iterationsCount / threadsCount);
                lock (bestMove)
                {
                    if (curBestMove.Item2 > bestMove.Item2)
                        bestMove = curBestMove;
                }
            });
            var newRocket = rocket.Move(bestMove.Item1, level);
            return newRocket;
        }
    }
}