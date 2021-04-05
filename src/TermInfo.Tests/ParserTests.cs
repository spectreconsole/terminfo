using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using TermInfo;
using Xunit;

namespace TermInfo.Tests
{
    public sealed class ParserTests
    {
        [Fact]
        public void Should_Read_Terminfo_File()
        {
            // Given
            var stream = EmbeddedResourceReader.LoadResourceStream("TermInfo.Tests/Data/xterm+256color");

            // When
            var info = TermInfoData.Read(stream);

            // Then
            info.MaxColors.ShouldBe(256);
            info.AutoLeftMargin.ShouldBe(false);
            info.OrigColors.ShouldBe("\u001b]104\a");
        }
    }

    public static class EmbeddedResourceReader
    {
        public static Stream LoadResourceStream(string resourceName)
        {
            if (resourceName is null)
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            var assembly = Assembly.GetCallingAssembly();
            resourceName = resourceName.Replace("/", ".");

            return assembly.GetManifestResourceStream(resourceName);
        }

        public static Stream LoadResourceStream(Assembly assembly, string resourceName)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (resourceName is null)
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            resourceName = resourceName.Replace("/", ".");
            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}
