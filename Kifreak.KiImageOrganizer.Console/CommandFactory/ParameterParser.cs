namespace Kifreak.KiImageOrganizer.Console.CommandFactory
{
    public class ParameterParser
    {
        public string[] GetParameters(string[] arguments, int skip, int minimum)
        {
            if (arguments.Length < minimum)
            {
                return null;
            }
            string[] toLabels = new string[arguments.Length - skip];
            var iteration = 0;
            for (var i = skip; i < arguments.Length; i++)
            {
                toLabels[iteration] = arguments[i];
                iteration++;
            }

            return toLabels;

        }
    }
}