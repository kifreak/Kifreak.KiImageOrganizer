using System.Reflection;
using Autofac;
using Kifreak.KiImageOrganizer.Console.Actions;
using Kifreak.KiImageOrganizer.Console.CommandFactory;
using Kifreak.KiImageOrganizer.Console.Commands;
using Kifreak.KiImageOrganizer.Console.Formatters;
using Kifreak.KiImageOrganizer.Console.Services;
using Module = Autofac.Module;

namespace Kifreak.KiImageOrganizer.Console.Configuration
{
    public class ModuleInjection : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //TODO: Reduce number of register change by Assembly (Like all Commands must be register always)
            builder.RegisterType<ActionParser>().As<IActionParser>();
            builder.RegisterType<ActionService>();
            builder.RegisterType<GeoService>().As<IGeoService>();
            builder.RegisterType<MetadataService>().As<IMetadataService>();
            builder.RegisterType<NotFoundCommand>();
            builder.RegisterType<OrganizerImagesCommand>();
            builder.RegisterType<RemoveCacheCommand>();
            builder.RegisterType<RenameFilesCommands>();
            builder.RegisterType<AddTagsCommand>();
            builder.RegisterType<CommandParser>();
            builder.RegisterType<ParameterParser>();
            builder.RegisterType<City>();
            builder.RegisterType<ByDate>();
            builder.RegisterType<Noop>();
            builder.RegisterType<MainFolder>();
            builder.RegisterType<FileFormatters>();
            builder.RegisterType<FolderFormatters>();
            builder.RegisterType<JustValueFormatters>();
        }
    }
}