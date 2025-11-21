// Ported from https://github.com/xo/terminfo/blob/master/gen.go

using System.IO.Compression;
using Humanizer;
using Scriban;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.IO;
using TermInfo;

namespace Generator.Commands;

public sealed class GenerateCommand : AsyncCommand<GenerateCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandArgument(0, "<OUTPUT>")]
        public string Output { get; set; } = null!;

        [CommandOption("-i|--input <PATH>")]
        public string? Input { get; set; }
    }

    public override async Task<int> ExecuteAsync(
        CommandContext context, Settings settings,
        CancellationToken cancellationToken)
    {
        var templates = new FilePath[]
        {
            $"Templates/{nameof(TermInfoCaps)}.template",
            $"Templates/{nameof(TermInfoDesc)}.Generated.template",
        };

        // Read all capabilities
        var capabilities = await ReadCapabilities(settings);

        // Ensure the output directory exist
        var output = new DirectoryPath(settings.Output);
        if (!Machine.Shared.FileSystem.Directory.Exists(settings.Output))
        {
            Machine.Shared.FileSystem.Directory.Create(settings.Output);
        }

        // Render all templates
        foreach (var templatePath in templates)
        {
            var outputFilename = templatePath.GetFilename().ChangeExtension(".cs");
            AnsiConsole.MarkupLine($"Generating [yellow]{outputFilename}[/]...");

            // Parse the Scriban template.
            var template = Template.Parse(await File.ReadAllTextAsync(templatePath.FullPath, cancellationToken));

            // Render the template with the model.
            var result = await template.RenderAsync(new { Capabilities = capabilities });

            // Write output to file
            var file = output.CombineWithFilePath(outputFilename);
            await File.WriteAllTextAsync(file.FullPath, result, cancellationToken);
        }

        return 0;
    }

    private async Task<Capabilities> ReadCapabilities(Settings settings)
    {
        var result = new List<Capability>();
        var caps = await ReadCapabilitiesString(settings);

        AnsiConsole.MarkupLine($"Parsing [yellow]{CapsPath}[/]...");

        var lines = caps.Split(['\n'], StringSplitOptions.RemoveEmptyEntries);

        var indices = new Dictionary<CapabilityType, int>
        {
            { CapabilityType.Bool, 0 },
            { CapabilityType.Num, 0 },
            { CapabilityType.String, 0 },
        };

        foreach (var line in lines)
        {
            if (line.StartsWith("#"))
            {
                continue;
            }

            var columns = line.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
            var name = columns[0].Trim();
            var description = columns[7].Trim();

            var type = columns[2] switch
            {
                "bool" => CapabilityType.Bool,
                "num" => CapabilityType.Num,
                "str" => CapabilityType.String,
                _ => throw new InvalidOperationException($"Unknown caps type '{columns[2]}' ({line})"),
            };

            var typeName = type switch
            {
                CapabilityType.Bool => "bool",
                CapabilityType.Num => "num",
                CapabilityType.String => "string",
                _ => throw new NotSupportedException(),
            };

            var prefix = type switch
            {
                CapabilityType.Bool => "indicates",
                CapabilityType.Num => "is",
                CapabilityType.String => "is the",
                _ => throw new NotSupportedException(),
            };

            result.Add(new Capability
            {
                Name = name.Pascalize(),
                Variable = name,
                Type = type,
                Index = indices[type],
                Description = $"The {name} [{columns[0]}, {columns[1]}] {typeName} capability {prefix} {columns[7].Humanize(LetterCasing.LowerCase)}.",
            });

            indices[type] += 1;
        }

        var capabilities = new Capabilities(result);
        return capabilities;
    }

    private async Task<string> ReadCapabilitiesString(Settings settings)
    {
        await using var gzip = new GZipStream(await FetchSourcePackage(settings), CompressionMode.Decompress);
        using var reader = SharpCompress.Readers.Tar.TarReader.Open(gzip);
        while (reader.MoveToNextEntry())
        {
            if (reader.Entry.Key != CapsPath)
            {
                continue;
            }

            var output = new MemoryStream();
            reader.WriteEntryTo(output);

            output.Seek(0, SeekOrigin.Begin);
            var streamReader = new StreamReader(output);
            return await streamReader.ReadToEndAsync();
        }

        throw new InvalidOperationException($"Could not find '{CapsPath}'");
    }

    private const string NCursesUrl = "https://ftp.gnu.org/pub/gnu/ncurses/ncurses-6.5.tar.gz";
    private const string NCursesFilename = "ncurses-6.5.tar.gz";
    private const string CapsPath = "ncurses-6.5/include/Caps";

    private async Task<Stream> FetchSourcePackage(Settings settings)
    {
        var input = string.IsNullOrEmpty(settings.Input)
            ? Machine.Shared.Environment.WorkingDirectory
            : new DirectoryPath(settings.Input);

        var file = Machine.Shared.FileSystem.File.Retrieve(input.CombineWithFilePath(NCursesFilename));
        if (!file.Exists)
        {
            await AnsiConsole.Status()
                .StartAsync($"Downloading [yellow]{NCursesUrl}[/]...", async ctx =>
                {
                    using var http = new HttpClient();
                    using var httpStream = await http.GetStreamAsync(NCursesUrl);
                    using var outStream = file.OpenWrite();

                    await httpStream.CopyToAsync(outStream);
                });
        }

        return file.OpenRead();
    }
}