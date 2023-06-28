using System.Reflection;
using Autofac;
using Kifreak.KiImageOrganizer.CoreConsole.Actions;
using Kifreak.KiImageOrganizer.CoreConsole.Services;
using Module = Autofac.Module;

namespace Kifreak.KiImageOrganizer.CoreConsole.Configuration
{
    public class ModuleInjection : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var dataAccess = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(dataAccess)
                .Where(t => t.Name.EndsWith("Command") || t.Name.EndsWith("Parser") || t.Name.EndsWith("Formatters"))
                .PublicOnly();
            builder.RegisterType<ActionParser>().As<IActionParser>();
            builder.RegisterType<ActionService>().As<IActionService>();
            builder.RegisterType<GeoService>().As<IGeoService>();
            builder.RegisterType<MetadataService>().As<IMetadataService>();
            builder.RegisterType<RegexService>().As<IRegexService>();
            builder.RegisterType<City>();
            builder.RegisterType<ByDate>();
            builder.RegisterType<Noop>();
            builder.RegisterType<MainFolder>();
        }
    }
}