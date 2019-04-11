namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public class MainFolder : SubFolders
    {
        private readonly string _path;


        public MainFolder(string path)
        {
            _path = path;
        }

        public override string GetSubFolder()
        {
            return _path;
        }
    }
}