namespace Keep_Silence
{
    public interface ICreature
    {
        CreatureCommand MakeStep(Game game);
        int GetNoiseLevel();
        void ActionInConflict(ICreature conflictedObject, Game game);
        string GetImageFileName();
        string GetHitImageFileName();
    }
}
