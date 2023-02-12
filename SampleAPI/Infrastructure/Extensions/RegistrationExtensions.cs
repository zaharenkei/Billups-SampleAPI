using FluentValidation;
using SampleAPI.Handlers;
using SampleAPI.Services.DataProviders;
using SampleAPI.Services.ExternalClients;
using SampleAPI.Services.ExternalClients.Models;
using SampleAPI.Services.ResultProvider;
using SampleAPI.Handlers.Models;
using SampleAPI.Validators;
using SampleAPI.Validators.CustomFactory;

namespace SampleAPI.Infrastructure.Extensions
{
    public static class RegistrationExtensions
    {
        internal static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RandomClientSettings>(configuration.GetSection("Clients:Random"));

            services.RegisterImplementations(typeof(IHandler<,>));
            services.RegisterImplementations(typeof(IHandler<>));
            services.AddSingleton<IGameChoicesProvider, GameChoicesProvider>();
            services.AddSingleton<IUserStatisticsProvider, UserStatisticsProvider>();
            services.AddSingleton<IRandomClient, RandomClient>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IResultProvider>(sp =>
            {
                var httpContext = sp.GetService<IHttpContextAccessor>()?.HttpContext;
                return httpContext != null && httpContext.Request.Query.ContainsKey("unfair")
                    ? new UnfairResultProvider()
                    : new ResultProvider();
            });

            services.AddSingleton<ICustomValidatorFactory, CustomValidatorFactory>();
            services.AddScoped<IValidator<PlayGameRequest>, GameRequestValidator>();
        }

        private static void RegisterImplementations(this IServiceCollection services, Type @interface)
        {
            foreach (var implementation in @interface.GetImplementations())
            {
                var fullInterface = implementation.GetInterfaces()
                    .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == @interface);
                services.AddTransient(fullInterface, implementation);
            }
        }
    }
}
