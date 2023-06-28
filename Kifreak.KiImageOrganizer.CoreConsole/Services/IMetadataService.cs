namespace Kifreak.KiImageOrganizer.CoreConsole.Services
{
    public interface IMetadataService
    {
        void SetFileInformation(string file);

        string GetKey(string key);

        void WriteInformation(string file, string keyWords);
    }
}