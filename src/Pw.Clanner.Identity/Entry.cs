using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ory.Hydra.Client.Api;
using Pw.Clanner.Identity.Common.Behaviours;
using Pw.Clanner.Identity.Common.Interfaces;
using Pw.Clanner.Identity.Infrastructure.Persistence;
using Pw.Clanner.Identity.Infrastructure.Services;

namespace Pw.Clanner.Identity;

public static class Entry
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(Entry).Assembly);

            options.AddOpenRequestPreProcessor(typeof(LoggingBehaviour<>));
            options.AddOpenBehavior(typeof(HydraChallengeBehaviour<,>));
            options.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            options.AddOpenBehavior(typeof(PerformanceBehaviour<,>));
            options.AddOpenBehavior(typeof(UnhandledExceptionBehaviour<,>));
        });
        services.AddValidatorsFromAssembly(typeof(Entry).Assembly);

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["App:DbConnectionString"];
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IDomainEventService, DomainEventService>();
        services.AddSingleton<ICurrentHydraChallenge, CurrentHydraChallenge>();

        var hydraBaseUrl = configuration["Hydra:BaseUrl"]!;

        services.AddScoped<IOAuth2ApiAsync>(_ => new OAuth2Api(hydraBaseUrl));

        return services;
    }
}