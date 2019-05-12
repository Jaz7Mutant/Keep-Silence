using System.Drawing;

namespace Keep_Silence
{
    public class CreatureAnimation
    {
        public bool HitAnimation;
        public Point Location;
        public Point TargetLogicalLocation;
        public ICreature Creature;
        public CreatureCommand Command;
    }
}
