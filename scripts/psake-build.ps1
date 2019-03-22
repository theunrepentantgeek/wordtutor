##
## Psake build for the WordTutor
##

properties {
    $baseDir = resolve-path ..\
    $scriptsDir = resolve-path $baseDir\scripts
    $srcDir = resolve-path $baseDir\src
    $testsDir = resolve-path $baseDir\tests
}

# Do our integration build 
Task Integration.Build -Depends Clean.BuildDir, Clean.SourceDir, Compile, Unit.Tests, Coverage.Report

## --------------------------------------------------------------------------------
##   Prerequisite Targets
## --------------------------------------------------------------------------------
## Tasks used to ensure that prerequisites are available when needed. 

Task Requires.DotNetExe {
    $script:dotnetExe = (get-command dotnet -ErrorAction SilentlyContinue).Path

    if ($dotnetExe -eq $null) {
        $script:dotnetExe = resolve-path $env:ProgramFiles\dotnet\dotnet.exe -ErrorAction SilentlyContinue
    }

    if ($dotnetExe -eq $null) {
        throw "Failed to find dotnet.exe"
    }

    Write-Output "Dotnet executable: $dotnetExe"
}

Task Requires.ReportGenerator -Depends Requires.DotNetExe {

    $toNatural = { [regex]::Replace($_, '\d+', { $args[0].Value.PadLeft(20) }) }

    # dotnet returns a list of the form "info : label : folder"
    # So we strip "info : " from the start of each line
    Write-Output "[-] Finding NuGet caches via dotnet.exe"
    $locals = & $dotnetExe nuget locals all --list | % { $_.SubString(7) }

    foreach($local in $locals)
    {
        $index = $local.IndexOf(":")
        $folder = $local.Substring($index + 1).Trim()

        $exePath = `
            get-childitem $folder\reportgenerator\*\ReportGenerator.exe `
                -ErrorAction SilentlyContinue -Recurse `
            | Sort-Object $toNatural | select-object -last 1

        if ($exePath -ne $null)
        {
            Write-Output "[+] ReportGenerator found at $exePath"
            $script:reportGeneratorExe = $exePath
            break
        }
        else 
        {
            Write-Output "[x] ReportGenerator not found within $folder"
        }
    }

    if ($reportGeneratorExe -eq $null)
    {
        throw "Failed to find ReportGenerator.exe"
    }
}

## --------------------------------------------------------------------------------
##   Cleaning Targets
## --------------------------------------------------------------------------------
## Tasks used to clean up 

Task Clean.SourceDir {

    Write-Info "Cleaning $srcDir"
    remove-item $srcDir\*\bin\* -recurse -ErrorAction SilentlyContinue
    remove-item $srcDir\*\obj\* -recurse -ErrorAction SilentlyContinue
    remove-item $srcDir\*\publish\* -recurse -ErrorAction SilentlyContinue

    Write-Info "Cleaning $testsDir"
    remove-item $testsDir\*\bin\* -recurse -ErrorAction SilentlyContinue
    remove-item $testsDir\*\obj\* -recurse -ErrorAction SilentlyContinue
    remove-item $testsDir\*\publish\* -recurse -ErrorAction SilentlyContinue
}

Task Clean.BuildDir {

    $script:buildDir = join-path $baseDir "build"

    Write-Host "Build output folder: $buildDir"

    if (test-path $buildDir) {
        remove-item $buildDir -recurse -force -ErrorAction SilentlyContinue
    }

    mkdir $buildDir -ErrorAction SilentlyContinue | Out-Null
}

Task Clean.CoverageReportDir -Depends Clean.BuildDir {

    $script:coverageReportDir = join-path $buildDir coverage
    Write-Host "Coverage report folder: $coverageReportDir"

    if (test-path $coverageReportDir) {
        remove-item $coverageReportDir -recurse -force -ErrorAction SilentlyContinue
    }

    mkdir $coverageReportDir -ErrorAction SilentlyContinue | Out-Null
}

## --------------------------------------------------------------------------------
##   Build Targets
## --------------------------------------------------------------------------------
## Tasks used to perform steps of the actual build

Task Compile -Depends Requires.DotNetExe {
    
    $solution = resolve-path $baseDir\*.sln
    Write-Info "Solution is $solution"
    Write-Host

    & $dotNetExe build $solution
}

Task Unit.Tests -Depends Requires.DotNetExe, Compile {

    foreach ($project in (resolve-path $testsDir\*\*.csproj)) {
        $projectName = (get-item $project).BaseName
        Write-SubtaskName $projectName
        exec {
            & $dotnetExe test $project /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:Exclude="[xunit*]*%2c[*.Tests]*"
        }
    }
}

Task Coverage.Report -Depends Requires.ReportGenerator, Clean.CoverageReportDir, Unit.Tests {

    exec {
        & $reportGeneratorExe -reports:$testsDir\*\*.opencover.xml -targetdir:$coverageReportDir
    }

    $openCoverIndex = resolve-path $coverageReportDir\index.htm
    & $openCoverIndex
}

## --------------------------------------------------------------------------------
##   Support Functions
## --------------------------------------------------------------------------------

formatTaskName { 
    param($taskName) 

    $width = (get-host).UI.RawUI.WindowSize.Width - 2
    if ($width -eq $null -or $width -gt 100) {
        $width = 100
    }

    $divider = "=" * $width
    $now = get-date -format "HH:mm:ss"
    $spacer = " " * ( $width - $taskName.Length - 14 )
    return "`r`n$divider`r`n  $taskName $spacer $now`r`n$divider`r`n"
} 

function Write-SubtaskName($subtaskName) {
    $divider = "-" * ($subtaskName.Length + 4)
    Write-Output "$divider`r`n  $subtaskName`r`n$divider`r`n"
}

function Write-Info($message) {
    Write-Host "[*] $message"
}

function Write-Warning($message) {
    Write-Host "[!] $message"
}
