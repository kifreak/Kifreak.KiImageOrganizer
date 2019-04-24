using MetadataExtractor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kifreak.KiImageOrganizer.Console.Services
{
    public class MetadataService : IMetadataService
    {
        private List<Directory> _metadataInfo;

        public void SetFileInformation(string file)
        {
            _metadataInfo = ImageMetadataReader.ReadMetadata(file).ToList();
        }

        public string GetKey(string key)
        {
            Directory sectionValue = _metadataInfo.FirstOrDefault(t => t.Tags.Any(x => x.Name.Contains(key)));

            if (sectionValue == null) return string.Empty;
            Tag tag = sectionValue.Tags.FirstOrDefault(t =>
                t.Name.Equals(key, StringComparison.OrdinalIgnoreCase));

            return tag?.Description;
        }
    }
}