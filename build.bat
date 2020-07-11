@echo off
dotnet publish -r win-x64 -c Release --nologo -v m -p:PublishSingleFile=true
dotnet publish -r win-x86 -c Release --nologo -v m -p:PublishSingleFile=true
dotnet publish -r linux-x64 -c Release --nologo -v m -p:PublishSingleFile=true
dotnet publish -r linux-arm64 -c Release --nologo -v m -p:PublishSingleFile=true
dotnet publish -r osx-x64 -c Release --nologo -v m -p:PublishSingleFile=true
pause