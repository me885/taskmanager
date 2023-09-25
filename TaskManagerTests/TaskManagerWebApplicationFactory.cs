using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskManagerApi;

namespace TaskManagerTests;

public class TaskManagerWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.Remove(services.Single(x => x.ServiceType == typeof(ITaskDatabase)));
            services.Remove(services.Single(x => x.ServiceType == typeof(IUserDatabase)));

            services.AddSingleton<ITaskDatabase, InMemoryTaskDb>();
            services.AddSingleton<IUserDatabase, InMemoryUserDb>();
        });

        return base.CreateHost(builder);
    }
}