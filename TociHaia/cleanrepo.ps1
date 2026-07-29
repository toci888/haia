[CmdletBinding(SupportsShouldProcess)]
param(
    [Parameter()]
    [string]$RepoPath = $PSScriptRoot
)

$ErrorActionPreference = "Stop"

try {
    $resolvedRepoPath = (Resolve-Path -LiteralPath $RepoPath).Path

    if ([string]::IsNullOrWhiteSpace($resolvedRepoPath)) {
        throw "Could not determine repository path."
    }

    $driveRoot = [System.IO.Path]::GetPathRoot($resolvedRepoPath)

    if ($resolvedRepoPath -eq $driveRoot) {
        throw "For safety, this script cannot run on a drive root."
    }

    Write-Host "Repository: $resolvedRepoPath" -ForegroundColor Cyan
    Write-Host "Searching for bin directories..." -ForegroundColor Cyan

    # Collect all bin directories before deleting anything.
    $binDirectories = @(
        Get-ChildItem `
            -LiteralPath $resolvedRepoPath `
            -Directory `
            -Recurse `
            -Force `
            -ErrorAction SilentlyContinue |
        Where-Object { $_.Name -ieq "bin" } |
        Sort-Object { $_.FullName.Length } -Descending
    )

    if ($binDirectories.Count -eq 0) {
        Write-Host "No bin directories found." -ForegroundColor Green
        exit 0
    }

    Write-Host ""
    Write-Host "Found bin directories: $($binDirectories.Count)" -ForegroundColor Yellow

    foreach ($directory in $binDirectories) {
        Write-Host "  $($directory.FullName)" -ForegroundColor DarkYellow
    }

    Write-Host ""

    foreach ($directory in $binDirectories) {
        if ($PSCmdlet.ShouldProcess(
            $directory.FullName,
            "Delete bin directory and all its contents"
        )) {
            Remove-Item `
                -LiteralPath $directory.FullName `
                -Recurse `
                -Force `
                -ErrorAction Stop

            Write-Host "Deleted: $($directory.FullName)" -ForegroundColor Green
        }
    }

    Write-Host ""
    Write-Host "Done. Deleted bin directories: $($binDirectories.Count)" `
        -ForegroundColor Green
}
catch {
    Write-Error "Repository cleanup failed: $($_.Exception.Message)"
    exit 1
}