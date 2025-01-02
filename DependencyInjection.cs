using AwsS3Api.Interfaces;
using AwsS3Api.Services;
using AwsS3Api.Settings;

namespace AwsS3Api;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IObjectStorage, S3ObjectStorageManager>();
        
        services.Configure<S3Settings>(bind => configuration.GetSection("S3Settings").Bind(bind));
        
        return services;
    }
}