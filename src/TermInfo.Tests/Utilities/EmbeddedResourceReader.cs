using System.Reflection;

namespace TermInfo.Tests;

public static class EmbeddedResourceReader
{
    public static Stream LoadResourceStream(string resourceName)
    {
        ArgumentNullException.ThrowIfNull(resourceName);

        var assembly = Assembly.GetCallingAssembly();
        resourceName = resourceName.Replace("/", ".");

        var stream = assembly.GetManifestResourceStream(resourceName);
        return stream ?? throw new InvalidOperationException($"Could not load file stream '{resourceName}'");
    }
}