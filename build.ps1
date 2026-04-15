$ProjectDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$OutputDir = Join-Path $ProjectDir "GUI\Lab4Timp\bin\Debug\net10.0-windows"
$MenuDLLPath = Join-Path $OutputDir "MenuLib.dll"
$AuthDLLPath = Join-Path $OutputDir "AuthLib.dll"

if (!(Test-Path $OutputDir)) {
    New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null
}

Remove-Item -Force -ErrorAction SilentlyContinue $MenuDLLPath
Remove-Item -Force -ErrorAction SilentlyContinue $AuthDLLPath

$env:CGO_ENABLED = "1"
$env:GOOS = "windows"
$env:GOARCH = "amd64"

# MenuLib
Push-Location (Join-Path $ProjectDir "MenuLib")
go mod download
go mod tidy
go build -buildmode=c-shared -o "$MenuDLLPath" ./cmd/menu_export
if ($LASTEXITCODE -ne 0) {
    Write-Host "MenuLib build failed"
    Pop-Location
    Read-Host "Press Enter to exit"
    exit 1
}
Pop-Location

# AuthLib
Push-Location (Join-Path $ProjectDir "AuthLib")
go mod download
go mod tidy
go build -buildmode=c-shared -o "$AuthDLLPath" ./cmd/auth_export
if ($LASTEXITCODE -ne 0) {
    Write-Host "AuthLib build failed"
    Pop-Location
    Read-Host "Press Enter to exit"
    exit 1
}
Pop-Location

Write-Host ""
Write-Host "MenuLib.dll saved to: $MenuDLLPath"
Write-Host "AuthLib.dll saved to: $AuthDLLPath"
Write-Host ""
Write-Host "Press Enter to exit..." -ForegroundColor Cyan
Read-Host