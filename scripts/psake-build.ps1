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
Task Integration.Build -Depends Clean.SourceFolder, Unit.Tests

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

## --------------------------------------------------------------------------------
##   Cleaning Targets
## --------------------------------------------------------------------------------
## Tasks used to clean up 

Task Clean.SourceFolder {

    Write-Info "Cleaning $srcDir"
    remove-item $srcDir\*\bin\* -recurse -ErrorAction SilentlyContinue
    remove-item $srcDir\*\obj\* -recurse -ErrorAction SilentlyContinue
    remove-item $srcDir\*\publish\* -recurse -ErrorAction SilentlyContinue

    Write-Info "Cleaning $testsDir"
    remove-item $testsDir\*\bin\* -recurse -ErrorAction SilentlyContinue
    remove-item $testsDir\*\obj\* -recurse -ErrorAction SilentlyContinue
    remove-item $testsDir\*\publish\* -recurse -ErrorAction SilentlyContinue
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
            & $dotnetExe test $project /p:CollectCoverage=true /p:Exclude="[xunit*]*%2c[*.Tests]*"
        }
    }
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
