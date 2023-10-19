using CreditAgricoleSdk.Interfaces;
using CreditAgricoleSdk.Repository;
using CreditAgricoleSdk.Repository.Interfaces;
using CreditAgricoleSdk.Serializer;
using CreditAgricoleSdk.Serializer.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CreditAgricoleSdk;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCreditAgricoleSdk(this IServiceCollection services)
    {
        services
            .AddScoped<IHttpClient, HttpClient>()
            .AddScoped<IKeypadRepository, KeypadRepository>()
            .AddScoped<IRegionalBankRepository, RegionalBankRepository>()
            .AddScoped<IAccountSerializer, AccountSerializer>()
            .AddScoped<IOperationSerializer, OperationSerializer>()
            .AddScoped<IOperationsInfoSerializer, OperationsInfoSerializer>()
        ;

        return services;
    }
}