using Game.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.GeneticAlgorithm
{
    public class Neighbour
    {
        public List<int> Sequence { get; set; }

        public Neighbour(List<int> sequence)
        {
            this.Sequence = sequence;
        }

        public double GetFitness()
        {
            var totalDistance = 0.0;

            for (int i = 1; i < this.Sequence.Count(); i++)
            {
                var fromTown = TownHelper.TownPositions[Sequence[i - 1]];
                var toTown = TownHelper.TownPositions[Sequence[i]];

                var x = toTown.X - fromTown.X;
                var y = toTown.Y - fromTown.Y;

                var d = Math.Sqrt(x * x + y * y);

                totalDistance += d;
            }

            return totalDistance;
        }
    }
}
