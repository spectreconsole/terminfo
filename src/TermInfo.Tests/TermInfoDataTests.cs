using Shouldly;
using Xunit;

namespace TermInfo.Tests;

public sealed class TermInfoDataTests
{
    [Fact]
    public void Should_Read_Standard_Format()
    {
        // Given
        var stream = EmbeddedResourceReader.LoadResourceStream($"TermInfo.Tests/Data/windows-ansi");

        // When
        var info = TermInfoDesc.Load(stream);

        // Then
        info.Names.Length.ShouldBe(2);
        info.Names[0].ShouldBe("ansi");
        info.Names[1].ShouldBe("ansi/pc-term compatible with color");
        info.ClearScreen.ShouldBe("[H[J");
    }

    [Theory]
    [InlineData("xterm+256color", 256)]
    [InlineData("xterm+88color", 88)]
    public void Should_Read_MaxColors(string terminfo, int expected)
    {
        // Given
        var stream = EmbeddedResourceReader.LoadResourceStream($"TermInfo.Tests/Data/{terminfo}");

        // When
        var info = TermInfoDesc.Load(stream);

        // Then
        info.MaxColors.ShouldBe(expected);
    }

    [Fact]
    public void Should_Read_Extended_Capabilities()
    {
        // Given
        var stream = EmbeddedResourceReader.LoadResourceStream("TermInfo.Tests/Data/eterm-256color");

        // When
        var info = TermInfoDesc.Load(stream);

        // Then
        info.Names.Length.ShouldBe(2);
        info.Names[0].ShouldBe("Eterm-256color");
        info.Names[1].ShouldBe("Eterm with xterm 256-colors");
        info.Extended.Count.ShouldBe(26);
        info.Extended.GetBoolean("AX").ShouldBe(true);
        info.Extended.GetBoolean("XT").ShouldBe(true);
        info.Extended.GetString("kUP").ShouldBe("\u001b[a");
    }

    [Fact]
    public void Should_Read_Extended__Capabilities_Without_String_Values()
    {
        // Given
        var stream = EmbeddedResourceReader.LoadResourceStream("TermInfo.Tests/Data/linux");

        // When
        var info = TermInfoDesc.Load(stream);

        // Then
        info.Names.Length.ShouldBe(2);
        info.Names[0].ShouldBe("linux");
        info.Names[1].ShouldBe("linux console");
        info.Extended.Count.ShouldBe(10);
        info.Extended.GetBoolean("AX").ShouldBe(true);
        info.Extended.GetBoolean("G0").ShouldBe(false);
        info.Extended.GetBoolean("XT").ShouldBe(false);
        info.Extended.GetNum("U8").ShouldBe(1);
    }

    [Theory]
    [InlineData("AX", true)]
    [InlineData("ax", null)]
    public void Should_Consider_Extended_Caps_Case_Sensitive(string key, bool? expected)
    {
        // Given
        var stream = EmbeddedResourceReader.LoadResourceStream("TermInfo.Tests/Data/linux");

        // When
        var info = TermInfoDesc.Load(stream);

        // Then
        info.Extended.GetBoolean(key).ShouldBe(expected);
    }
}