$ErrorActionPreference = "Stop"
Push-Location $PSScriptRoot

$manifest = Get-Content "thunderstore\manifest.json" | ConvertFrom-Json
$version = $manifest.version_number
Write-Host "Building SWIP v$version..."

dotnet build src/SWIP.csproj -c Release
if ($LASTEXITCODE -ne 0) { throw "Build failed" }

$dll = "src\bin\Release\netstandard2.1\SWIP.dll"
if (-not (Test-Path $dll)) { throw "SWIP.dll not found" }

Write-Host "Packaging for Thunderstore..."
$buildDir = "build"
if (Test-Path $buildDir) { Remove-Item $buildDir -Recurse -Force }
New-Item -ItemType Directory -Path $buildDir | Out-Null

Copy-Item $dll "$buildDir\SWIP.dll"
Copy-Item "thunderstore\manifest.json" $buildDir
Copy-Item "thunderstore\README.md" $buildDir
Copy-Item "thunderstore\icon.png" $buildDir
if (Test-Path "thunderstore\CHANGELOG.md") { Copy-Item "thunderstore\CHANGELOG.md" $buildDir }

$zipPath = "SWIP-$version.zip"
if (Test-Path $zipPath) { Remove-Item $zipPath }
Compress-Archive -Path "$buildDir\*" -DestinationPath $zipPath

Remove-Item $buildDir -Recurse -Force
Write-Host "Done! Package: $zipPath"

Pop-Location
