using System;
using System.Drawing;
using System.Windows.Forms;

namespace Keep_Silence
{
    public class Player : ICreature
    {
        public Point Position;
        public Directions Direction = Directions.Right;
        public bool ChangedDirection;
        public int NoiseLevel;
        public double HealthPoints;
        private int ticks;
        public int LightningRadius = 3;
        private const int TicksBeforeIdle = 7;
        private const int IdleNoiseLevel = 2;
        private const int NoisePerStep = 10;
        private const int NoisePerHit = 13;

        public int GetNoiseLevel() => NoiseLevel;

        public CreatureCommand MakeStep(Game game)
        {
            var shiftX = 0;
            var shiftY = 0;
            ChangedDirection = false;
            
            switch (game.KeyPressed)
            {
                //TODO Убрать Keys, Выделить в метод
                case Keys.W:
                    shiftY = -1;
                    if (Direction != Directions.Up)
                    { 
                        ChangedDirection = true;
                        shiftY = 0;
                    }
                    Direction = Directions.Up;
                    break;
                case Keys.S:
                    shiftY = 1;
                    if (Direction != Directions.Down)
                    {
                        ChangedDirection = true;
                        shiftY = 0;
                    }
                    Direction = Directions.Down;
                    break;
                case Keys.D:
                    shiftX = 1;
                    if (Direction != Directions.Right)
                    {
                        ChangedDirection = true;
                        shiftX = 0;
                    }
                    Direction = Directions.Right;
                    break;
                case Keys.A:
                    shiftX = -1;
                    if (Direction != Directions.Left)
                    {
                        ChangedDirection = true;
                        shiftX = 0;
                    }
                    Direction = Directions.Left;
                    break;

                case Keys.F:
                    InteractWithEnvironment(game);
                    return new CreatureCommand {HitAnimation = false, Target = Position};
                case Keys.Escape:
                    game.Pause();
                    return new CreatureCommand {HitAnimation = false, Target = Position};
                case Keys.Space:
                    MakeHit(game);
                    return new CreatureCommand {HitAnimation = true, Target = Position};
            }
            
            if (!game.IsStepCorrect(Position, new Point(Position.X + shiftX, Position.Y)))
                shiftX = 0;
            if (!game.IsStepCorrect(Position, new Point(Position.X, Position.Y + shiftY)))
                shiftY = 0;

            if (shiftX != 0 || shiftY != 0)
            {
                NoiseLevel = NoisePerStep;
                ticks = 0;
            }
            else
            {
                if (ticks > TicksBeforeIdle)
                NoiseLevel = IdleNoiseLevel;
            }

            ticks++;

            return new CreatureCommand
            {
                HitAnimation = false,
                Target = new Point(Position.X + shiftX, Position.Y + shiftY)
            };
        }

        public void ChangeHealthPoints(double points, Game game)
        {
            HealthPoints += points;
            HealthPoints = HealthPoints >= 100 ? 100 : HealthPoints;
            if (HealthPoints <= 0)
                game.GameOver();
        }

        public void ActionInConflict(ICreature conflictedObject, Game game)
        {
            if (conflictedObject is Monster)
            {
                ChangeHealthPoints(Monster.DamageToPlayer, game);
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

        public string GetImageFileName() => "Player.png";

        public string GetHitImageFileName() => "PlayerHit.png";

        private void InteractWithEnvironment(Game game)
        {
            var hitPoint = GetHitPoint();
            if (game.CurrentRoom.Map[hitPoint.X, hitPoint.Y] is Chest
                || game.CurrentRoom.Map[hitPoint.X, hitPoint.Y] is Door)
                game.CurrentRoom.Map[hitPoint.X, hitPoint.Y].InteractWithPlayer(game);
        }

        private void MakeHit(Game game)
        {
            ticks = 0;
            NoiseLevel = NoisePerHit;
            var hitPoint = GetHitPoint();
            var target = game.CurrentRoom.Monsters.Find(x => x.Position == hitPoint);
            target?.ActionInConflict(this, game);
        }
    }
}
