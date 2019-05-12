using System;
using System.Drawing;

namespace Keep_Silence
{
    public class Player : ICreature
    {
        public Point Position;
        private Directions Direction = Directions.Right;
        public int NoiseLevel;
        public double HealthPoints;
        private int ticks;  //todo причесать константы
        public int LightningRadius = 1;
        private const int TicksBeforeIdle = 7;
        private const int IdleNoiseLevel = 2;
        private const int NoisePerStep = 10;
        private const int NoisePerHit = 13;

        public int GetNoiseLevel() => NoiseLevel;

        public string GetImageFileName() => "Player.png";

        public string GetHitImageFileName() => "PlayerHit.png";

        //todo Метод для замены фонарика
        //todo разряжение аккума

        public CreatureCommand MakeStep(Game game)
        {
            var shiftX = 0;
            var shiftY = 0;
            var newDirection = Directions.None;
            switch (game.GetPlayerAction())
            {
                case PlayerActions.MoveUp:
                    shiftY = Direction == Directions.Up ? -1 : 0;
                    newDirection = Directions.Up;
                    break;
                case PlayerActions.MoveDown:
                    shiftY = Direction == Directions.Down ? 1 : 0;
                    newDirection = Directions.Down;
                    break;
                case PlayerActions.MoveRight:
                    shiftX = Direction == Directions.Right ? 1 : 0;
                    newDirection = Directions.Right;
                    break;
                case PlayerActions.MoveLeft:
                    shiftX = Direction == Directions.Left ? -1 : 0;
                    newDirection = Directions.Left;
                    break;
                case PlayerActions.Interact:
                    InteractWithEnvironment(game);
                    return new CreatureCommand {HitAnimation = false, Target = Position};
                case PlayerActions.Hit:
                    MakeHit(game);
                    return new CreatureCommand {HitAnimation = true, Target = Position};
            }
            if (!game.IsStepCorrect(Position, new Point(Position.X + shiftX, Position.Y)))
                shiftX = 0;
            if (!game.IsStepCorrect(Position, new Point(Position.X, Position.Y + shiftY)))
                shiftY = 0;
            var turn = Game.GetImageRotation(newDirection, Direction);
            Direction = newDirection == Directions.None ? Direction : newDirection;
            UpdateNoiseLevel(shiftX, shiftY);
            
            return new CreatureCommand
            {
                HitAnimation = false,
                Target = new Point(Position.X + shiftX, Position.Y + shiftY),
                Rotate = turn
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

        private void UpdateNoiseLevel(int shiftX, int shiftY)
        {
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
            ticks = 0;
            NoiseLevel = NoisePerHit;
            var hitPoint = GetHitPoint();
            var target = game.CurrentRoom.Monsters.Find(x => x.Position == hitPoint);
            target?.ActionInConflict(this, game);
        }
    }
}
