using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keep_Silence
{
    public class Monster : ICreature
    {
        public Point Position;
        public double NoiseLevel;
        private int _biteLoading;
        private const double IdleNoiseLevel = 5;
        private const int TicksBeforeBite = 8;
        private const double NoisePerStep = 10;
        public CreatureCommand MakeStep(Game game)
        {
            NoiseLevel = IdleNoiseLevel;
            var distanceToPlayer = game.GetDistanceBetweenPoints(Position, game.Player.Position);

            if (distanceToPlayer <= 1)
            {
                if (_biteLoading < TicksBeforeBite)
                {
                    _biteLoading++;
                    return new CreatureCommand(){InteractWithPlayer = false, target = Position};
                }

                _biteLoading = 0;
                return new CreatureCommand(){InteractWithPlayer = true, target = Position};
            }

            if (game.Player.GetNoiseLevel() < distanceToPlayer)
            {
                return new CreatureCommand() { InteractWithPlayer = false, target = Position };
            }

            //TODO: Dijkstra path finder
            NoiseLevel = NoisePerStep;
            //TODO: Find path to player
            return new CreatureCommand() {InteractWithPlayer = false, target = Position + new Size(1, 1)}; 
        }

        public int GetDrawingPriority() => 5;

        public double GetNoiseLevel() => NoiseLevel;

        public void ActionInConflict(ICreature conflictedObject, Game game)
        {
            if (conflictedObject is Player)
            {
                game.CurrentRoom.Monsters.Remove(this);
            }
        }
    }
}
