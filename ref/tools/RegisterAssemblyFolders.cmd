@echo 
set REG=%WINDIR%\System32\reg.exe
if (%PROCESSOR_ARCHITECTURE%)==(AMD64) set REG=%WINDIR%\SysWow64\reg.exe

set BASEDIR=C:\Prj\SmartExpert\BCL.CoreMin\code\main\build\bin\Debug
echo Registering SmartExpert.BCL.CoreMin assembly folders (using %REG%)
%REG% add "HKLM\SOFTWARE\Microsoft\.NETFramework\v3.5\AssemblyFoldersEx\SmartExpert.BCL.CoreMin" /ve /d "%BASEDIR%\net35" /f
%REG% add "HKLM\SOFTWARE\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\SmartExpert.BCL.CoreMin" /ve /d "%BASEDIR%\net40" /f
%REG% add "HKLM\SOFTWARE\Microsoft\.NETFramework\v4.5.50709\AssemblyFoldersEx\SmartExpert.BCL.CoreMin" /ve /d "%BASEDIR%\net45" /f

if (%PROCESSOR_ARCHITECTURE%)==(AMD64) (
	rem When running MSBuild 64bit version and targeting v3.5 MSBuild will only lock in the 64bit regpath => Add in 64bit regpath AssemblyFoldersEx ref to CoreMin
	rem Only .NET 3.5 assembly version must be added. If Targeting v4.0 und v4.5, MSBuild 64bit version will also look inside the 32bit (SysWow64) regpathes
	%WINDIR%\System32\reg.exe add "HKLM\SOFTWARE\Microsoft\.NETFramework\v3.5\AssemblyFoldersEx\SmartExpert.BCL.CoreMin" /ve /d "%BASEDIR%\net35" /f
)

