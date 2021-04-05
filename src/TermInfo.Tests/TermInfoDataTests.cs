using Shouldly;
using Xunit;

namespace TermInfo.Tests
{
    public sealed class TermInfoDataTests
    {
        [Theory]
        [InlineData("xterm+256color", 256)]
        [InlineData("xterm+88color", 88)]
        public void Should_Read_MaxColors(string terminfo, int expected)
        {
            // Given
            var stream = EmbeddedResourceReader.LoadResourceStream($"TermInfo.Tests/Data/{terminfo}");

            // When
            var info = TermInfoData.Read(stream);

            // Then
            info.MaxColors.ShouldBe(expected);
        }
    }
}
