using System;
using System.Drawing;
using System.Windows.Forms;

namespace Keep_Silence
{
    public class Player : ICreature
    {
        public Point Position;
        public Directions Direction = Directions.Right;
        public double NoiseLevel;
        public double HealthPoints;
        private const double IdleNoiseLevel = 2;
        private const double NoisePerStep = 10;
        private const double NoisePerHit = 13;
        private const double DamageFromMonster = 60;

        public int GetDrawingPriority() => 1;

        public double GetNoiseLevel() => NoiseLevel;

        public CreatureCommand MakeStep(Game game)
        {
            var shiftX = 0;
            var shiftY = 0;
            switch (game.KeyPressed)
            {
                case Keys.W:
                    shiftY = -1;
                    Direction = Directions.Up;
                    break;
                case Keys.S:
                    shiftY = 1;
                    Direction = Directions.Down;
                    break;
                case Keys.D:
                    shiftX = 1;
                    Direction = Directions.Right;
                    break;
                case Keys.A:
                    shiftX = -1;
                    Direction = Directions.Left;
                    break;

                case Keys.F:
                    InteractWithEnvironment(game);
                    return new CreatureCommand {HitAnimation = false, target = Position};
                case Keys.Escape:
                    game.Pause();
                    return new CreatureCommand {HitAnimation = false, target = Position};
                case Keys.Space:
                    MakeHit(game);
                    return new CreatureCommand {HitAnimation = true, target = Position};
            }

            if (!game.IsStepCorrect(Position, new Point(Position.X + shiftX, Position.Y)))
                shiftX = 0;
            if (!game.IsStepCorrect(Position, new Point(Position.X, Position.Y + shiftY)))
                shiftY = 0;

            if (shiftX != 0 || shiftY != 0)
                NoiseLevel = NoisePerStep;
            else NoiseLevel = IdleNoiseLevel;

            return new CreatureCommand
            {
                HitAnimation = false,
                target = new Point(Position.X + shiftX, Position.Y + shiftY)
            };
        }

        public void ChangeHealthPoints(double points, Game game)
        {
            HealthPoints += points;
            HealthPoints %= 100;
            if (HealthPoints <= 0)
                game.GameOver();
        }

        public void ActionInConflict(ICreature conflictedObject, Game game)
        {
            if (conflictedObject is Monster)
            {
                ChangeHealthPoints(DamageFromMonster, game);
            }
        }

        public Point GetHitPoint()
        {
            switch (Direction)
            {
                case Directions.Up:
                    return new Point(Position.X, Position.Y - 1);
                case Directions.Down:
                    return new Point(Position.X, Position.Y + 1);
                case Directions.Right:
                    return new Point(Position.X + 1, Position.Y);
                case Directions.Left:
                    return new Point(Position.X - 1, Position.Y);
                default:
                    throw new ArgumentOutOfRangeException("Incorrect hit direction");
            }
        }

        private void InteractWithEnvironment(Game game)
        {
            var hitPoint = GetHitPoint();
            if (game.CurrentRoom.Map[hitPoint.X, hitPoint.Y] is Chest
                || game.CurrentRoom.Map[hitPoint.X, hitPoint.Y] is Door)
                game.CurrentRoom.Map[hitPoint.X, hitPoint.Y].InteractWithPlayer(game);
        }

        private void MakeHit(Game game)
        {
            NoiseLevel = NoisePerHit;
            var hitPoint = GetHitPoint();
            var target = game.CurrentRoom.Monsters.Find(x => x.Position == hitPoint);
            target?.ActionInConflict(this, game);
        }
    }
}
