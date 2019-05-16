namespace Keep_Silence
{
    public interface IEnvironment
    {
        double Illumination { get; set; }
        void InteractWithPlayer(Game game);
        string GetImageFileName();
    }
}
