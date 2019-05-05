using MetadataExtractor;
using System;
using System.Collections.Generic;
using System.Linq;
using Kifreak.KiImageOrganizer.Console.Helpers;
using XperiCode.JpegMetadata;

namespace Kifreak.KiImageOrganizer.Console.Services
{
    public class MetadataService : IMetadataService
    {
        private List<Directory> _metadataInfo;

        public void SetFileInformation(string file)
        {
            try
            {
                _metadataInfo = ImageMetadataReader.ReadMetadata(file).ToList();
            }
            catch (Exception ex)
            {
                ConsoleHelper.Error($"Can't read metadata from file {file}. Error: {ex.Message}");
            }
        }

        public string GetKey(string key)
        {
            if (_metadataInfo == null)
            {
                return null;

            }
            Directory sectionValue = _metadataInfo.FirstOrDefault(t => t.Tags.Any(x => x.Name.Contains(key)));

            if (sectionValue == null) return string.Empty;
            Tag tag = sectionValue.Tags.FirstOrDefault(t =>
                t.Name.Equals(key, StringComparison.OrdinalIgnoreCase));

            return tag?.Description;
        }

        public void WriteInformation(string file, string keyWords)
        {
            var adapter = new JpegMetadataAdapter(file);
            adapter.Metadata.Keywords = keyWords.Split(',');
            bool saved = adapter.Save();
        }
    }
}