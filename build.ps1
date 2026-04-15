$ProjectDir = Split-Path -Parent $MyInvocation.MyCommand.Path | Split-Path -Parent
$OutputDir = Join-Path $ProjectDir "GUI\Lab4Timp\bin\Debug\net10.0-windows"
$MenuDLLPath = Join-Path $OutputDir "MenuLib.dll"
$AuthDLLPath = Join-Path $OutputDir "AuthLib.dll"

if (!(Test-Path $OutputDir)) {
    New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null
}

Remove-Item -Force -ErrorAction SilentlyContinue $MenuDLLPath
Remove-Item -Force -ErrorAction SilentlyContinue $AuthDLLPath

Push-Location $ProjectDir

$env:CGO_ENABLED = "1"
$env:GOOS = "windows"
$env:GOARCH = "amd64"

go mod download
go mod tidy

go build -buildmode=c-shared -o "$MenuDLLPath" ./MenuLib/cmd/menu_export

if ($LASTEXITCODE -ne 0) {
    Write-Host "MenuLib build failed"
    Pop-Location
    Read-Host "Press Enter to exit"
    exit 1
}

go build -buildmode=c-shared -o "$AuthDLLPath" ./AuthLib/cmd/auth_export

if ($LASTEXITCODE -ne 0) {
    Write-Host "AuthLib build failed"
    Pop-Location
    Read-Host "Press Enter to exit"
    exit 1
}

Pop-Location

Write-Host ""
Write-Host "MenuLib.dll saved to: $(Resolve-Path $MenuDLLPath)"
Write-Host "AuthLib.dll saved to: $(Resolve-Path $AuthDLLPath)"
Write-Host ""
Write-Host "Press Enter to exit..." -ForegroundColor Cyan
Read-Host