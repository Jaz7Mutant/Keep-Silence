using System.Collections.Generic;
using System.Drawing;

namespace Keep_Silence
{
    public class GameState
    {
        public List<CreatureAnimation> Animations = new List<CreatureAnimation>();
        public const int CellSize = 32;

        public void PerformAct(Game game)
        {
            Animations.Clear();
            var playerStep = game.Player.MakeStep(game);
            Animations.Add(new CreatureAnimation
            {
                HitAnimation = playerStep.HitAnimation, Location = ConvertPointToImageSize(game.Player.Position),
                TargetLogicalLocation = ConvertPointToImageSize(playerStep.Target),
                Creature = game.Player,
                Command = playerStep
            });
            game.Player.Position = playerStep.Target;
            game.CurrentRoom.LightenNewArea(game.Player.LightningRadius,game.Player.Position);
            foreach (var monster in game.CurrentRoom.Monsters)
            {
                var monsterStep = monster.MakeStep(game);
                Animations.Add(new CreatureAnimation
                {
                    HitAnimation = monsterStep.HitAnimation, Location = ConvertPointToImageSize(monster.Position),
                    TargetLogicalLocation = ConvertPointToImageSize(monsterStep.Target),
                    Creature = monster,
                    Command = monsterStep
                });
                monster.Position = monsterStep.Target;
            }
        }

        public Point ConvertPointToImageSize(Point point) => new Point(point.X*CellSize, point.Y*CellSize);
    }
}
