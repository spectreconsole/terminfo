using System.Diagnostics;

namespace Generator.Commands;

[DebuggerDisplay("{Type,nq}:{Name,nq} = {Description, nq}")]
public sealed class Capability
{
    public required string Name { get; init; }
    public required string Variable { get; init; }
    public required CapabilityType Type { get; init; }
    public required string Description { get; init; }
    public required int Index { get; init; }
}