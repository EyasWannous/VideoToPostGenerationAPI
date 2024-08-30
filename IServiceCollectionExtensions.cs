namespace VideoToPostGenerationAPI;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddCorsDevelopmentPolicy(this IServiceCollection services)
    {
        services.AddCors(opt => opt
            .AddPolicy("Ahmad", policy => policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                )
            );

        return services;
    }


}