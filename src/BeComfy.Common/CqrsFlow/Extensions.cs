using System.Reflection;
using Autofac;
using BeComfy.Common.CqrsFlow.Dispatcher;
using BeComfy.Common.CqrsFlow.Handlers;

namespace BeComfy.Common.CqrsFlow
{
    public static class Extensions
    {
        public static void AddDispatcher(this ContainerBuilder builder)
        {
            builder.RegisterType<CommandDispatcher>().As<ICommandDispatcher>();
        }

        public static void AddHandlers(this ContainerBuilder builder)
        {
            var assembly = Assembly.GetCallingAssembly();
            builder.RegisterAssemblyTypes(assembly)
                .AsClosedTypesOf(typeof(ICommandHandler<>))
                .InstancePerDependency();
        }
    }
}