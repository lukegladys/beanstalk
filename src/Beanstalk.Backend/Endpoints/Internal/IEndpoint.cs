namespace Beanstalk.Backend.Endpoints.Internal;

public interface IEndpoint
{
    public static abstract void AddServices(IServiceCollection serviceCollection, IConfiguration configuration);
    public static abstract void DefineEndpoints(IEndpointRouteBuilder app);
}