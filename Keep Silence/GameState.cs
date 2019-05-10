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
                Creature = game.Player
            });
            game.Player.Position = playerStep.Target;
            //if (playerStep.HitAnimation && game.CurrentRoom.Monsters.Find(x => x.Position == playerStep.Target) != null)
            //{
            //    game.CurrentRoom.Monsters.Find(x => x.Position == playerStep.Target).Dead(game.Player, game);
            //}
            foreach (var monster in game.CurrentRoom.Monsters)
            {
                var monsterStep = monster.MakeStep(game);
                //if (monsterStep.HitAnimation)
                //    game.Player.ChangeHealthPoints(monster.DamageToPlayer, game);
                Animations.Add(new CreatureAnimation
                {
                    HitAnimation = monsterStep.HitAnimation, Location = ConvertPointToImageSize(monster.Position),
                    TargetLogicalLocation = ConvertPointToImageSize(monsterStep.Target),
                    Creature = monster
                });
                monster.Position = monsterStep.Target;
            }
        }

        public Point ConvertPointToImageSize(Point point) => new Point(point.X*CellSize, point.Y*CellSize);
    }
}
