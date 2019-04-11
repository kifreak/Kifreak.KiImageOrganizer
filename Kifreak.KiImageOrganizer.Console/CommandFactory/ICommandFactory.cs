namespace Kifreak.KiImageOrganizer.Console.CommandFactory
{
    public interface ICommandFactory
    {
        string CommandName { get; }
        string Description { get; }
        ICommand MakeCommand(string[] arguments);
    }
}