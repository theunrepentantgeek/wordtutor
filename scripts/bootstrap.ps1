function TryLoad-Psake($path) {

    $toNatural = { [regex]::Replace($_, '\d+', { $args[0].Value.PadLeft(20) }) }

    $psakePath = `
        get-childitem $path\psake*\psake.psm1 `
            -ErrorAction SilentlyContinue -Recurse `
        | Sort-Object $toNatural | select-object -last 1

        if ($psakePath -eq $null) {
        Write-Output "[x] Psake not found within $path"
    } else {
        import-module $psakePath
    }
}

function TryLoad-Psake-ViaNuGetCache()
{
    $locals = $null

    $nuget = get-command nuget -ErrorAction SilentlyContinue
    $dotnet = get-command dotnet -ErrorAction SilentlyContinue

    if ($nuget -ne $null) {
        # nuget returns a list of the form "label : folder"
        Write-Output "[-] Finding NuGet caches via nuget.exe"
        $locals = & $nuget locals all -list
    } elseif ($dotnet -ne $null) {
        # dotnet returns a list of the form "info : label : folder"
        # So we strip "info : " from the start of each line
        Write-Output "[-] Finding NuGet caches via dotnet.exe"
        $locals = & $dotnet nuget locals all --list | % { $_.SubString(7) }
    }

    foreach($local in $locals)
    {
        $index = $local.IndexOf(":")
        $folder = $local.Substring($index + 1).Trim()
        TryLoad-Psake $folder
    }
}

if ((get-module psake) -eq $null) {
    TryLoad-Psake .\lib\psake
}

if ((get-module psake) -eq $null) {
    # Don't have psake loaded, try to load it from PowerShell's default module location
    import-module psake -ErrorAction SilentlyContinue
}

if ((get-module psake) -eq $null) {
    # Not yet loaded, try to load it from the packages folder
    TryLoad-Psake ".\packages\"
}

if ((get-module psake) -eq $null) {
    # Not yet loaded, try to load it from the chocolatey installation library
    TryLoad-Psake $env:ProgramData\chocolatey\lib
}

$env:DOTNET_PRINT_TELEMETRY_MESSAGE = "false"

if ((get-module psake) -eq $null) {
    # Still not loaded, let's look in the various NuGet caches
    TryLoad-Psake-ViaNuGetCache
}

$psake = get-module psake
if ($psake -ne $null) {
    $psakePath = $psake.Path
    Write-Output "[+] Psake loaded from $psakePath"
}
else {
    Write-Output "[!]"
    Write-Output "[!] ***** Unable to load PSake *****"
    Write-Output "[!]"
    throw "PSake not loaded"
}
