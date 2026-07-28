namespace Toci.Haia.Studio.Api.Features.Authentication;

public interface IStudioBootstrapConsole
{
    string? ReadLine();

    string ReadSecret();

    void Write(string text);

    void WriteLine(string text);
}
