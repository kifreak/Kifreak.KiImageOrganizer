using System.Collections.Generic;
using System.Linq;
using Kifreak.KiImageOrganizer.Console.CommandFactory;

namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public class ActionParser
    {
        private readonly IEnumerable<IActionOptions> _availableActions;

        public ActionParser(IEnumerable<IActionOptions> availableActions)
        {
            _availableActions = availableActions;
        }

        public IActionOptions ParseCommand(string name)
        {
            return _availableActions.FirstOrDefault(t => t.Options.Contains(name));
        }
    }

    public interface IActionOptions
    {
        string[] Options { get; }
    }
}