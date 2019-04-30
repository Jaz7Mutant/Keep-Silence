using System.Drawing;

namespace Keep_Silence
{
    public interface IEnvironment
    {
        double Illumination { get; set; }
        bool InteractWithPlayer(Game game);
    }
}
