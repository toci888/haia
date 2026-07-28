namespace Toci.Haia.Studio.Api.Features.Authentication;

public interface IStudioAdminBootstrapService
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}
