param()

$ErrorActionPreference = 'Stop'

$scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectRoot = Resolve-Path (Join-Path $scriptRoot '..')
$generatedEntities = Join-Path $projectRoot 'Entities/Generated'
$generatedContext = Join-Path $projectRoot 'Context/Generated'
$projectPath = Join-Path $projectRoot 'Toci.Haia.Database.Persistence.csproj'

if (-not (Test-Path $projectPath))
{
	throw "Project file not found: $projectPath"
}

$solutionRoot = Resolve-Path (Join-Path $projectRoot '..')

$connectionString = $env:HAIA_DB_CONNECTION_STRING
if ([string]::IsNullOrWhiteSpace($connectionString))
{
	throw 'Missing HAIA_DB_CONNECTION_STRING environment variable.'
}

Push-Location $solutionRoot
try
{
	dotnet tool restore

	if (Test-Path $generatedEntities)
	{
		Remove-Item $generatedEntities -Recurse -Force
	}
	if (Test-Path $generatedContext)
	{
		Remove-Item $generatedContext -Recurse -Force
	}

	New-Item -ItemType Directory -Path $generatedEntities -Force | Out-Null
	New-Item -ItemType Directory -Path $generatedContext -Force | Out-Null

	dotnet ef dbcontext scaffold "$connectionString" Npgsql.EntityFrameworkCore.PostgreSQL `
		--project "$projectPath" `
		--context HaiaDbContext `
		--output-dir Entities/Generated `
		--context-dir Context/Generated `
		--namespace Toci.Haia.Database.Persistence.Entities `
		--context-namespace Toci.Haia.Database.Persistence.Context `
		--no-onconfiguring `
		--schema identity `
		--schema onboarding `
		--schema humor `
		--schema studio `
		--schema learning `
		--schema ai `
		--schema audit `
		--force

	$generatedFiles = Get-ChildItem -Path $generatedEntities -Filter *.cs -File -ErrorAction Stop
	$entityCount = $generatedFiles.Count

	$leaks = Select-String -Path (Join-Path $projectRoot '**/*.cs') -Pattern 'Password=' -SimpleMatch -ErrorAction SilentlyContinue
	if ($leaks)
	{
		throw 'Generated source contains forbidden Password= fragment.'
	}

	$dbContextPath = Join-Path $generatedContext 'HaiaDbContext.cs'
	if (-not (Test-Path $dbContextPath))
	{
		throw 'HaiaDbContext.cs was not generated.'
	}

	$onConfiguringLeak = Select-String -Path $dbContextPath -Pattern 'OnConfiguring|UseNpgsql\(' -ErrorAction SilentlyContinue
	if ($onConfiguringLeak)
	{
		throw 'Generated DbContext contains forbidden OnConfiguring/UseNpgsql fragment.'
	}

	dotnet build "$projectPath" -c Debug

	Write-Host ("Generated entity classes: {0}" -f $entityCount)
}
finally
{
	Pop-Location
}
