namespace Toci.Haia.Api.Common.Security;

public static class EmailMasking
{
    public static string Mask(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
        {
            return "***";
        }

        var parts = email.Split('@');
        var user = parts[0];
        var domain = parts[1];

        if (user.Length <= 2)
        {
            return "**@" + domain;
        }

        return user[..1] + new string('*', user.Length - 2) + user[^1] + "@" + domain;
    }
}
