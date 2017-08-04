@echo off

rem set PLATFRM="x86"
set PLATFRM="Any CPU"

nuget restore -PackagesDirectory packages packages.config
nuget restore -PackagesDirectory packages UT\packages.config

sn -k eLyKseeR.snk
sn -k UT\ut.snk

msbuild /t:clean /p:Configuration="Debug" /p:Platform=%PLATFRM% eLyKseeR-base.Win32.sln
msbuild /p:Configuration="Debug" /p:Platform=%PLATFRM% eLyKseeR-base.Win32.sln

copy ..\sharpPRNG_development\prngCpp\w32\Debug\prngCpp.dll UT\bin\Debug

packages\NUnit.Runners.lite.2.6.4.20150512\nunit-console.exe UT\bin\Debug\UT.exe

rem NUnit test results in: TestResult.xml

