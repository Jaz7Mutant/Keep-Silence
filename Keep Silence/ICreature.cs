namespace Keep_Silence
{
    public interface ICreature
    {
        CreatureCommand MakeStep(Game game);
        double GetNoiseLevel();
        void ActionInConflict(ICreature conflictedObject, Game game);
        string GetImageFileName();
        string GetHitImageFileName();
    }
}
