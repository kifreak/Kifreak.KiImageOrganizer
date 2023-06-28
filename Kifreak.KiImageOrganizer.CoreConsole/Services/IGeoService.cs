using Kifreak.KiImageOrganizer.CoreConsole.Models;
using System.Threading.Tasks;

namespace Kifreak.KiImageOrganizer.CoreConsole.Services
{
    public interface IGeoService
    {
        double ConvertCoordinates(double degrees, double minutes, double seconds);

        Task<OSMData> GetOsmData(Coordinates coordinates);
    }
}