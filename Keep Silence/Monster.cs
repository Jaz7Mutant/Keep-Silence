using System.Drawing;

namespace Keep_Silence
{
    public class Monster : ICreature
    {
        public Point Position;
        public double NoiseLevel;
        private int _biteLoading;
        private int _moveLoading;
        private const double IdleNoiseLevel = 5;
        private const int TicksBeforeBite = 10; //Todo Причесать константы
        private const int TicksBeforeMove = 5;
        private const double NoisePerStep = 10;
        public const double DamageToPlayer = 60;

        public double GetNoiseLevel() => NoiseLevel;

        public CreatureCommand MakeStep(Game game)
        {
            NoiseLevel = IdleNoiseLevel;
            var distanceToPlayer = game.GetDistanceBetweenPoints(Position, game.Player.Position);

            if (distanceToPlayer <= 1)
            {
                if (_biteLoading < TicksBeforeBite)
                {
                    _biteLoading++;
                    return new CreatureCommand(){Target = Position};
                }

                _biteLoading = 0;
                game.Player.ActionInConflict(this, game);
                return new CreatureCommand(){Target = Position, HitAnimation = true};
            }

            if (game.Player.GetNoiseLevel() < distanceToPlayer)
            {
                return new CreatureCommand() {Target = Position};
            }

            //TODO: Поиск в ширину
            NoiseLevel = NoisePerStep;
            var shiftX = game.Player.Position.X.CompareTo(Position.X);
            var shiftY = game.Player.Position.Y.CompareTo(Position.Y);
            var target = Position;
            if (shiftX != 0 && game.IsStepCorrect(Position, new Point(Position.X + shiftX, Position.Y)))
                target.X += shiftX;
            else if (shiftY != 0 && game.IsStepCorrect(Position, new Point(Position.X, Position.Y + shiftY)))
                target.Y += shiftY;
            if (_moveLoading < TicksBeforeMove)
            {
                _moveLoading++; //Todo Make KPACUBO
                target = Position;
            }
            else
            {
                _moveLoading = 0;
            }

            return new CreatureCommand() {Target = target}; 
        }

        public void ActionInConflict(ICreature conflictedObject, Game game)
        {
            if (conflictedObject is Player)
            {
                game.CurrentRoom.Monsters.Remove(this);
            }
        }

        public string GetImageFileName() => "Monster.png";

        public string GetHitImageFileName() => "MonsterHit.png";
    }
}
