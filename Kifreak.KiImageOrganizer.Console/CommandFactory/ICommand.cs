using System.Threading.Tasks;

namespace Kifreak.KiImageOrganizer.Console.CommandFactory
{
    public interface ICommand
    {
        Task Execute();

        bool Validate();
    }
}