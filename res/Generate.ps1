##########################################################
# Script that generates known terminfo capabilities
##########################################################

$Output = Join-Path $PSScriptRoot ".temp"
$Source = Join-Path $PSScriptRoot "/../src/TermInfo"

if(!(Test-Path $Output -PathType Container)) {
    New-Item -ItemType Directory -Path $Output | Out-Null
}

# Generate the files
Push-Location generator/Generator
&dotnet run "$Output" --input $Output
if(!$?) { 
    Pop-Location
    Throw "An error occured when generating code."
}
Pop-Location

# Copy the files to the correct location
Copy-Item  (Join-Path "$Output" "TermInfoCaps.cs") -Destination "$Source/TermInfoCaps.cs"
Copy-Item  (Join-Path "$Output" "TermInfoData.Generated.cs") -Destination "$Source/TermInfoData.Generated.cs"
