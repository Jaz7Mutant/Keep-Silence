using System;
using System.Drawing;

namespace Keep_Silence
{
    public class Player : ICreature
    {
        public Point Position;
        private Directions direction = Directions.Right;
        private int noiseLevel;
        private double healthPoints = 100;
        private int ticks; 
        private int lightningRadius = 1;
        private double flashlightPoints = 100;

        public int GetNoiseLevel() => noiseLevel;

        public double GetHealthPoints() => healthPoints;

        public int GetLightningRadius() => lightningRadius;

        public int GetFlashlightPoints() => (int)flashlightPoints;

        public string GetImageFileName() => "Player.png";

        public string GetHitImageFileName() => "PlayerHit.png";

        public CreatureCommand MakeStep(Game game)
        {
            var shiftX = 0;
            var shiftY = 0;
            var newDirection = Directions.None;
            switch (game.GetPlayerAction())
            {
                case PlayerActions.MoveUp:
                    //shiftY = direction == Directions.Up ? -1 : 0;
                    shiftY = -1;
                    newDirection = Directions.Up;
                    break;
                case PlayerActions.MoveDown:
                    //shiftY = direction == Directions.Down ? 1 : 0;
                    shiftY = 1;
                    newDirection = Directions.Down;
                    break;
                case PlayerActions.MoveRight:
                    //shiftX = direction == Directions.Right ? 1 : 0;
                    shiftX = 1;
                    newDirection = Directions.Right;
                    break;
                case PlayerActions.MoveLeft:
                    //shiftX = direction == Directions.Left ? -1 : 0;
                    shiftX = -1;
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
            var turn = Game.GetImageRotation(newDirection, direction);
            direction = newDirection == Directions.None ? direction : newDirection;
            UpdateNoiseLevel(shiftX, shiftY);
            UpdateFlashlight();
            
            return new CreatureCommand
            {
                HitAnimation = false,
                Target = new Point(Position.X + shiftX, Position.Y + shiftY),
                Rotate = turn
            };
        }

        public void ChangeHealthPoints(double points, Game game)
        {
            healthPoints += points;
            healthPoints = healthPoints >= 100 ? 100 : healthPoints;
            if (healthPoints <= 0)
                game.GameOver();
        }

        public void ChangeLightningRadius(int radius)
        {
            lightningRadius += radius;
            lightningRadius = lightningRadius <= 0 ? 0 : lightningRadius;
        }

        public void ChangeFlashlightPoints(double points)
        {
            flashlightPoints += points;
            flashlightPoints = flashlightPoints >= 100 
                ? 100 
                : flashlightPoints < 0 
                    ? 0 
                    : flashlightPoints;
            if (lightningRadius == 0 && flashlightPoints > 0)
                lightningRadius = 1;
        }

        public void ActionInConflict(ICreature conflictedObject, Game game)
        {
            if (conflictedObject is Monster)
            {
                ChangeHealthPoints(MonsterSettings.DamageToPlayer, game);
            }
        }

        private void UpdateFlashlight()
        {
            //TODO Добавить мигание
            ChangeFlashlightPoints(PlayerSettings.FlashlightChargeDecreasePerTick);
            if (flashlightPoints < 0.3)
                ChangeLightningRadius(-1000);
        }

        private Point GetHitPoint()
        {
            switch (direction)
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
                noiseLevel = PlayerSettings.NoisePerStep;
                ticks = 0;
            }
            else
            {
                if (ticks > PlayerSettings.TicksBeforeIdle)
                    noiseLevel = PlayerSettings.IdleNoiseLevel;
            }
            ticks++;
        }

        private void InteractWithEnvironment(Game game)
        {
            var hitPoint = GetHitPoint();
            if (!game.InBounds(hitPoint))
                return;
            if (game.CurrentRoom.Map[hitPoint.X, hitPoint.Y] is Chest
                || game.CurrentRoom.Map[hitPoint.X, hitPoint.Y] is Door)
                game.CurrentRoom.Map[hitPoint.X, hitPoint.Y].InteractWithPlayer(game);
        }

        private void MakeHit(Game game)
        {
            ticks = 0;
            noiseLevel = PlayerSettings.NoisePerHit;
            var hitPoint = GetHitPoint();
            var target = game.CurrentRoom.Monsters.Find(x => x.Position == hitPoint);
            target?.ActionInConflict(this, game);
        }
    }
}
