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
            var shiftX = game.Player.Position.X.CompareTo(Position.X);
            var shiftY = game.Player.Position.Y.CompareTo(Position.Y);
            var target = Position;
            if (shiftX != 0 && game.IsStepCorrect(Position, new Point(Position.X + shiftX, Position.Y)))
                target.X += shiftX;
            else if (shiftY != 0 && game.IsStepCorrect(Position, new Point(Position.X, Position.Y + shiftY)))
                target.Y += shiftY;
            return new CreatureCommand() {InteractWithPlayer = false, target = target}; 
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
