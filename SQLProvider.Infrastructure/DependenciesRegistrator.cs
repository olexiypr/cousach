using Microsoft.Extensions.DependencyInjection;

namespace SQLProvider.Infrastructure;

public class DependenciesRegistrator
{
    private readonly IServiceCollection _serviceCollection;
    public DependenciesRegistrator(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }
    public void Register()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        _serviceCollection.Scan(scan => scan.FromAssemblies(assemblies)
            .AddClasses(c => c.WithAttribute<DefaultTransientImplementation>())
            .AsImplementedInterfaces()
            .WithTransientLifetime());
    }
}