using System.Collections.Generic;

namespace Keep_Silence
{
    public class GameState
    {
        public List<CreatureAnimation> Animations = new List<CreatureAnimation>();
        public const int CellSize = 32;

        public void BeginAct(Game game)
        {
            Animations.Clear();
            var playerStep = game.Player.MakeStep(game);
            Animations.Add(new CreatureAnimation
            {
                HitAnimation = playerStep.HitAnimation, Location = game.Player.Position,
                TargetLogicalLocation = playerStep.target
            });
            foreach (var monster in game.CurrentRoom.Monsters)
            {
                var monsterStep = monster.MakeStep(game);
                Animations.Add(new CreatureAnimation
                {
                    HitAnimation = monsterStep.HitAnimation, Location = monster.Position,
                    TargetLogicalLocation = monsterStep.target
                });
            }
            //Player
            //Monsters
        }

        public void EndAct(Game game)
        {
            //Взаимодействия сущностей
        }
    }
}
