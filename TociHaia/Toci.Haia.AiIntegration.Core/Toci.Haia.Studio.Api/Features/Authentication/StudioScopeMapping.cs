namespace Toci.Haia.Studio.Api.Features.Authentication;

public static class StudioScopeMapping
{
    public const string StudioApiScope = "studio.api";
    public const string StudioRoleScope = "studio";

    public static string ToClaimScope(string roleScope)
    {
        if (string.Equals(roleScope, StudioRoleScope, StringComparison.OrdinalIgnoreCase))
        {
            return StudioApiScope;
        }

        return roleScope;
    }
}