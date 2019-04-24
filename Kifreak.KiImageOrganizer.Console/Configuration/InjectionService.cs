using Autofac;
using System;

namespace Kifreak.KiImageOrganizer.Console.Configuration
{
    public abstract class InjectionService
    {
        private readonly ContainerBuilder _builder;

        protected InjectionService()
        {
            _builder = new ContainerBuilder();
        }

        public void Execute()
        {
            Startup(Register);
        }

        protected abstract void Register(ContainerBuilder builder);

        private void Startup(Action<ContainerBuilder> registerMoreAssemblies)
        {
            _builder.RegisterAssemblyModules(typeof(ModuleInjection).Assembly);
            registerMoreAssemblies?.Invoke(_builder);
            Config.Container = _builder.Build();
        }
    }
}