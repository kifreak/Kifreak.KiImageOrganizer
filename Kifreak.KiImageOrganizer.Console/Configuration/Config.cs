using Autofac;
using Autofac.Core;
using Kifreak.KiImageOrganizer.Console.CommandFactory;
using Kifreak.KiImageOrganizer.Console.Commands;
using System;
using System.Collections.Generic;

namespace Kifreak.KiImageOrganizer.Console.Configuration
{
    public static class Config
    {
        private static UserConfig _userConfig;

        public static UserConfig UserConfig {
            get => _userConfig ?? (_userConfig = new UserConfig());
            set => _userConfig = value;
        }

        public static DateTime LastCallToOSM { get; set; }
        public static char AlternativeCharacter => '\\';
        public static IContainer Container { get; set; }

        public static IEnumerable<ICommandFactory> GetAvailableCommands()
        {
            return new ICommandFactory[]
            {
                Get<OrganizerImagesCommand>(), Get<RenameFilesCommand>(), Get<RemoveCacheCommand>(), Get<AddTagsCommand>(), Get<HelpCommand>()
            };
        }

        public static void Startup()
        {
            Injection injection = new Injection();
            injection.Execute();
        }

        public static T Get<T>(string[] keys, object[] values)
        {
            if (keys.Length != values.Length)
            {
                throw new Exception("Keys and values must have the same number of elements.");
            }
            List<Parameter> parameterList = new List<Parameter>();
            for (var i = 0; i < keys.Length; i++)
            {
                parameterList.Add(new NamedParameter(keys[i], values[i]));
            }

            return Container.Resolve<T>(parameterList);
        }

        public static T Get<T>(string key, object value)
        {
            return Get<T>(new[] { key }, new[] { value });
        }

        public static T Get<T>()
        {
            return Container.Resolve<T>();
        }
    }
}