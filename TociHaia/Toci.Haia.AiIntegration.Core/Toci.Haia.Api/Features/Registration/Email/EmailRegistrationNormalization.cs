using System.Globalization;

namespace Toci.Haia.Api.Features.Registration.Email;

public static class EmailRegistrationNormalization
{
    public static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();

    public static string NormalizeNickname(string nickname) => nickname.Trim().ToLowerInvariant();

    public static string NormalizeLanguage(string? language, EmailRegistrationOptions options)
    {
        if (string.IsNullOrWhiteSpace(language))
        {
            return options.DefaultLanguage;
        }

        return language.Trim();
    }

    public static bool IsValidEmail(string email)
    {
        try
        {
            _ = new System.Net.Mail.MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsSupportedLanguage(string language)
    {
        if (string.IsNullOrWhiteSpace(language))
        {
            return false;
        }

        try
        {
            _ = CultureInfo.GetCultureInfo(language);
            return true;
        }
        catch (CultureNotFoundException)
        {
            return false;
        }
    }
}
