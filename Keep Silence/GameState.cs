using System.Collections.Generic;

namespace Keep_Silence
{
    public class GameState
    {
        public List<CreatureAnimation> Animations = new List<CreatureAnimation>();

        public void BeginAct(Game game)
        {
            Animations.Clear();
            //Environment
            //Player
            //Monsters
        }

        public void EndAct(Game game)
        {
            //Взаимодействия сущностей
        }
    }
}
