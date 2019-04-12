using System;
using System.Collections.Generic;
using System.Linq;
using MetadataExtractor;

namespace Kifreak.KiImageOrganizer.Console.Services
{
    public class MetadataService
    {
        private readonly string _path;
        private readonly List<Directory> _metadataInfo;
        public MetadataService(string path)
        {
            _path = path;
            _metadataInfo =  ImageMetadataReader.ReadMetadata(path).ToList();
        }

        public string GetKey(string key)
        {
            MetadataExtractor.Directory sectionValue = _metadataInfo.FirstOrDefault(t => t.Tags.Any(x => x.Name.Contains(key)));
            
                if (sectionValue == null) return string.Empty;
                Tag tag = sectionValue?.Tags.FirstOrDefault(t =>
                    t.Name.Equals(key, StringComparison.OrdinalIgnoreCase));

                return tag?.Description;
            

        }
    }
}