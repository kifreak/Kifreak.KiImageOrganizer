using System.Threading.Tasks;

namespace Kifreak.KiImageOrganizer.CoreConsole.CommandFactory
{
    public interface ICommand
    {
        Task Execute();

        bool Validate();
    }
}