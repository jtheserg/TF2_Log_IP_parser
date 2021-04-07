$req_files_copy = 1
$clear_dir = 1
$publish_opinions = "-p:PublishSingleFile=true -p:SelfContained=true -p:PublishTrimmed=true"

IF (clear_dir == 1) {
    Write-Host "---CLEARING RELEASE DIR---"
    Remove-Item "bin\Release" -Recurse
    Remove-Item "bin\Release"
}

Write-Host "---win-x64---"
dotnet publish -r win-x64 -c Release --nologo -v m %publish_opinions% -o bin\Release\TF2_Log_IP_parser-win-x64
Write-Host "---win-x86---"
dotnet publish -r win-x86 -c Release --nologo -v m %publish_opinions% -o bin\Release\TF2_Log_IP_parser-win-x86
Write-Host "---linux-x64---"
dotnet publish -r linux-x64 -c Release --nologo -v m %publish_opinions% -o bin\Release\TF2_Log_IP_parser-linux-x64
Write-Host "---linux-arm64---"
dotnet publish -r linux-arm64 -c Release --nologo -v m %publish_opinions% -o bin\Release\TF2_Log_IP_parser-linux-arm64
Write-Host "---osx-x64---"
dotnet publish -r osx-x64 -c Release --nologo -v m %publish_opinions% -o bin\Release\TF2_Log_IP_parser-osx-x64

Write-Host "---GARBAGE DELETE---"
FOR (/d %%a in (bin\Release\netcoreapp*)) {
    Remove-Item "%%a" -Recurse
    Remove-Item "%%a"
}
DEL /F /Q /S ".\bin\Release\*.pdb" > nul

if (%req_files_copy%==1) {
    Write-Host "---COPY REQ FILES---"
    for (/d %%i in (bin\Release\*)) {
        copy ".\RequiredFilesForExport\" "%%i"
}}

Write-Host "---ZIP---"
FOR (/d %%b in (bin\Release\*)) {
	Compress-Archive -Force -CompressionLevel Optimal -Path ".\%%b" -DestinationPath ".\bin\Release\%%~nb.zip"
	Remove-Item "%%b" -Recurse
    Remove-Item "%%b"
	Write-Host "Archive \`".\bin\Release\%%~nb.zip\`" has (probably) been created and the source directory has been deleted."
}

ECHO ---END---
endlocal
pause