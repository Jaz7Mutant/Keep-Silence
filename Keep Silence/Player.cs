using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keep_Silence
{
    public class Player : ICreature
    {
        public CreatureCommand MakeStep(Game game)
        {
            throw new NotImplementedException();
        }

        public int GetDrawingPriority()
        {
            throw new NotImplementedException();
        }

        public double GetNoiseLevel()
        {
            throw new NotImplementedException();
        }

        public void ActionInConflict(ICreature conflictedObject, Game game)
        {
            throw new NotImplementedException();
        }
    }
}
