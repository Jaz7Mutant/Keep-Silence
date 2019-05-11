using System.Drawing;

namespace Keep_Silence
{
    public class Monster : ICreature
    {
        public Point Position;
        public int NoiseLevel;
        private int biteLoading;
        private int moveLoading;
        private double distanceToPlayer;
        private const int IdleNoiseLevel = 3;
        private const int TicksBeforeBite = 10; //Todo Причесать константы
        private const int TicksBeforeMove = 5;
        private const int NoisePerStep = 10;
        public const double DamageToPlayer = 60;

        public int GetNoiseLevel() => distanceToPlayer > NoiseLevel ? 0 : NoiseLevel;

        public CreatureCommand MakeStep(Game game)
        {
            NoiseLevel = IdleNoiseLevel;
            distanceToPlayer = game.GetDistanceBetweenPoints(Position, game.Player.Position);

            if (distanceToPlayer <= 1)
            {
                if (biteLoading < TicksBeforeBite)
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

            //TODO: Поиск в ширину
            NoiseLevel = NoisePerStep;
            var shiftX = game.Player.Position.X.CompareTo(Position.X);
            var shiftY = game.Player.Position.Y.CompareTo(Position.Y);
            var target = Position;
            if (shiftX != 0 && game.IsStepCorrect(Position, new Point(Position.X + shiftX, Position.Y)))
                target.X += shiftX;
            else if (shiftY != 0 && game.IsStepCorrect(Position, new Point(Position.X, Position.Y + shiftY)))
                target.Y += shiftY;
            if (moveLoading < TicksBeforeMove)
            {
                moveLoading++; //Todo Make KPACUBO
                target = Position;
            }
            else
            {
                moveLoading = 0;
            }

            return new CreatureCommand {Target = target}; 
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
