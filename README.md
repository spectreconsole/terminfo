# terminfo

A library to read compiled, binary terminfo descriptions.

_[![TermInfo NuGet Version](https://img.shields.io/nuget/v/TermInfo.svg?style=flat&label=NuGet%3A%20TermInfo)](https://www.nuget.org/packages/TermInfo)_

## Usage

```csharp
// Load the active terminfo description
var info = TermInfoDesc.Load();

// Load via terminfo name
var info = TermInfoDesc.Load("xterm-256color");

// Default capabilities are strongly typed
Debug.Assert(info.MaxColors == 256)

// But extended capabilities are also supported
bool? ax = info.Extended.GetBoolean("AX");
int? u8 = info.Extended.GetNum("U8");
string? kup = info.Extended.GetNum("kUP");
```

## Building

We're using [Cake](https://github.com/cake-build/cake) as a 
[dotnet tool](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools) 
for building. So make sure that you've restored Cake by running 
the following in the repository root:

```
> dotnet tool restore
```

After that, running the build is as easy as writing:

```
> dotnet cake
```

## Acknowledgement

This code is partly a port of https://github.com/xo/terminfo,
licensed under [MIT](https://github.com/xo/terminfo/blob/ca9a967f877831dd8742c136f5c19f82d03673f4/LICENSE).
