using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keep_Silence
{
    public interface ICreature
    {
        CreatureCommand MakeStep(Game game);
        int GetDrawingPriority();
        double GetNoiseLevel();
        void ActionInConflict(ICreature conflictedObject, Game game);
    }
}
