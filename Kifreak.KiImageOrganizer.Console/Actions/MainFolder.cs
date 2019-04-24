﻿using Kifreak.KiImageOrganizer.Console.Formatters;
using System.Threading.Tasks;

namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public class MainFolder : SubFolders
    {
        private readonly string _path;

        public MainFolder(string path)
        {
            _path = path;
        }

        public override Task<string> GetSubFolder(IFormatter formatter)
        {
            return Task.FromResult(formatter.Format(_path, string.Empty));
        }
    }
}