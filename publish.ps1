# PantryDesk Publish Script
# Creates single-file executables and demo files in dist/ folder

$ErrorActionPreference = "Stop"

Write-Host "Publishing PantryDesk..." -ForegroundColor Green

# Get repo root
$repoRoot = $PSScriptRoot
$distFolder = Join-Path $repoRoot "dist"

# Clean/create dist folder
Write-Host "Cleaning dist folder..." -ForegroundColor Yellow
if (Test-Path $distFolder) {
    Remove-Item -Path $distFolder -Recurse -Force
}
New-Item -ItemType Directory -Path $distFolder -Force | Out-Null

# Publish PantryDeskApp
Write-Host "Publishing PantryDeskApp..." -ForegroundColor Yellow
$appProject = Join-Path $repoRoot "src\PantryDeskApp\PantryDeskApp.csproj"
dotnet publish $appProject `
    -c Release `
    -r win-x64 `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -o (Join-Path $distFolder "PantryDeskApp")

# Publish PantryDeskSeeder
Write-Host "Publishing PantryDeskSeeder..." -ForegroundColor Yellow
$seederProject = Join-Path $repoRoot "src\PantryDeskSeeder\PantryDeskSeeder.csproj"
dotnet publish $seederProject `
    -c Release `
    -r win-x64 `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -o (Join-Path $distFolder "PantryDeskSeeder")

# Copy demo database and config into PantryDeskApp folder (portable demo)
Write-Host "Copying demo database and config..." -ForegroundColor Yellow
$appFolder = Join-Path $distFolder "PantryDeskApp"
$demoDb = Join-Path $repoRoot "demo_pantrydesk.db"
if (Test-Path $demoDb) {
    Copy-Item -Path $demoDb -Destination $appFolder -Force
    Write-Host "  Copied demo_pantrydesk.db to PantryDeskApp" -ForegroundColor Gray
} else {
    Write-Host "  Warning: demo_pantrydesk.db not found in repo root" -ForegroundColor Yellow
}

# Create demo config (relative path for portability)
$demoConfigPath = Join-Path $appFolder "PantryDesk.demo.config"
$demoConfigContent = "DemoDatabasePath = demo_pantrydesk.db"
Set-Content -Path $demoConfigPath -Value $demoConfigContent -Force
Write-Host "  Created PantryDesk.demo.config (relative path)" -ForegroundColor Gray

Write-Host ""
Write-Host "Publish complete!" -ForegroundColor Green
Write-Host "Output location: $distFolder" -ForegroundColor Cyan
Write-Host ""
Write-Host "Files created:" -ForegroundColor Cyan
Write-Host "  PantryDeskApp\PantryDeskApp.exe" -ForegroundColor Gray
Write-Host "  PantryDeskApp\PantryDesk.demo.config" -ForegroundColor Gray
Write-Host "  PantryDeskApp\demo_pantrydesk.db" -ForegroundColor Gray
Write-Host "  PantryDeskSeeder\PantryDeskSeeder.exe" -ForegroundColor Gray
