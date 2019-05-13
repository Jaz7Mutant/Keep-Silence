using System.Drawing;

namespace Keep_Silence
{
    public class Monster : ICreature
    {
        public Point Position;
        public int NoiseLevel;
        private double visibility;
        private Directions direction = Directions.Right;
        private int biteLoading;
        private int moveLoading;
        private double distanceToPlayer;

        public int GetNoiseLevel() => distanceToPlayer > NoiseLevel ? 0 : NoiseLevel;

        public double GetVisibility() => visibility;

        public string GetImageFileName() => "Monster.png";

        public string GetHitImageFileName() => "MonsterHit.png";

        public CreatureCommand MakeStep(Game game)
        {
            NoiseLevel = MonsterSettings.IdleNoiseLevel;
            distanceToPlayer = game.GetDistanceBetweenPoints(Position, game.Player.Position);
            visibility = distanceToPlayer <= game.Player.GetLightningRadius() + 1 ? 100 : 0;

            if (distanceToPlayer <= 1)
            {
                if (biteLoading < MonsterSettings.TicksBeforeBite)
                {
                    biteLoading++;
                    return new CreatureCommand {Target = Position};
                }
                biteLoading = 0;
                game.Player.ActionInConflict(this, game);
                return new CreatureCommand{Target = Position, HitAnimation = true};
            }

            biteLoading = 0;
            if (game.Player.GetNoiseLevel() < distanceToPlayer)
                return new CreatureCommand {Target = Position};

            NoiseLevel = MonsterSettings.NoisePerStep;
            GetNextPoint(game, out var target, out var turn);
            return new CreatureCommand {Target = target, Rotate = turn}; 
        }

        public void ActionInConflict(ICreature conflictedObject, Game game)
        {
            if (conflictedObject is Player)
            {
                game.CurrentRoom.Monsters.Remove(this);
            }
        }

        private void GetNextPoint(Game game, out Point target, out RotateFlipType turn)
        {
            //TODO Поиск в ширину
            var shiftX = game.Player.Position.X.CompareTo(Position.X);
            var shiftY = game.Player.Position.Y.CompareTo(Position.Y);
            target = Position;
            if (shiftX != 0 && game.IsStepCorrect(Position, new Point(Position.X + shiftX, Position.Y)))
                target.X += shiftX;
            else if (shiftY != 0 && game.IsStepCorrect(Position, new Point(Position.X, Position.Y + shiftY)))
                target.Y += shiftY;
            if (moveLoading < MonsterSettings.TicksBeforeMove)
            {
                moveLoading++;
                target = Position;
            }
            else
                moveLoading = 0;

            var newDirection = shiftX == 0
                ? shiftY > 0
                    ? Directions.Down
                    : Directions.Up
                : shiftX > 0
                    ? Directions.Right
                    : Directions.Left;
            turn = Game.GetImageRotation(newDirection, direction);
            direction = newDirection == Directions.None ? direction : newDirection;
        }
    }
}
