using Autofac;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Services;

namespace CleanArchitecture.Core;

public class DefaultCoreModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<ToDoItemSearchService>()
        .As<IToDoItemSearchService>().InstancePerLifetimeScope();
  }
}
