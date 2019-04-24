using Kifreak.KiImageOrganizer.Console.Models;
using System.Threading.Tasks;

namespace Kifreak.KiImageOrganizer.Console.Services
{
    public interface IGeoService
    {
        double ConvertCoordinates(double degrees, double minutes, double seconds);

        Task<OSMData> GetOsmData(Coordinates coordinates);
    }
}