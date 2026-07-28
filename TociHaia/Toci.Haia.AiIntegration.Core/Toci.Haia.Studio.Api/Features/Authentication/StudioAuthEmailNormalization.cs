namespace Toci.Haia.Studio.Api.Features.Authentication;

public static class StudioAuthEmailNormalization
{
    public static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();
}
