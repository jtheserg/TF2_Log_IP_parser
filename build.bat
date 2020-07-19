@echo off
SET transfer_web=http://localhost:8080/
SET /A req_files_copy=1
SET /A clear_dir=1
SET publish_opinions=-p:PublishSingleFile=true -p:SelfContained=true -p:PublishTrimmed=true

setlocal enabledelayedexpansion

IF %clear_dir%==1 (
ECHO ---CLEARING RELEASE DIR---
DEL /F /Q /S "bin\Release\*" > nul
RMDIR /Q /S "bin\Release\" > nul
)

ECHO ---win-x64---
dotnet publish -r win-x64 -c Release --nologo -v m %publish_opinions% -o bin\Release\TF2_Log_IP_parser-win-x64
ECHO ---win-x86---
dotnet publish -r win-x86 -c Release --nologo -v m %publish_opinions% -o bin\Release\TF2_Log_IP_parser-win-x86
ECHO ---linux-x64---
dotnet publish -r linux-x64 -c Release --nologo -v m %publish_opinions% -o bin\Release\TF2_Log_IP_parser-linux-x64
ECHO ---linux-arm64---
dotnet publish -r linux-arm64 -c Release --nologo -v m %publish_opinions% -o bin\Release\TF2_Log_IP_parser-linux-arm64
ECHO ---osx-x64---
dotnet publish -r osx-x64 -c Release --nologo -v m %publish_opinions% -o bin\Release\TF2_Log_IP_parser-osx-x64

ECHO ---GARBAGE DELETE---
FOR /d %%a in (bin\Release\netcoreapp*) do (
DEL /F /Q /S "%%a" > nul
RMDIR /Q /S "%%a" > nul
)
DEL /F /Q /S ".\bin\Release\*.pdb" > nul

if %req_files_copy%==1 (
ECHO ---COPY REQ FILES---
for /d %%i in (bin\Release\*) do (
copy ".\RequiredFilesForExport\" "%%i"
))

ECHO ---ZIP---
FOR /d %%b in (bin\Release\*) do (
	powershell Compress-Archive -Force -CompressionLevel Optimal -Path ".\%%b" -DestinationPath ".\bin\Release\%%~nb.zip"
	DEL /F /Q /S ".\%%b\" > nul
	RMDIR /Q /S ".\%%b\" > nul
	ECHO Archive ^".\bin\Release\%%~nb.zip^" has ^(probably^) been created and the source directory has been deleted.
)

ECHO ---SEND---
FOR /f %%f in ('dir /b .\bin\Release\') do (
	curl -XPUT -H "Max-Downloads: 1" -H "Max-Days: 1" --data-binary "@.\bin\Release\%%f" %transfer_web%%%f
	ECHO.
)

ECHO ---END---
endlocal
pause