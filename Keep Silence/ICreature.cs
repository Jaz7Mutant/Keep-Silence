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
