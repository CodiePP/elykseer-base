#!/bin/bash

set -e

PLATFRM="Any CPU"

nuget restore -PackagesDirectory packages packages.config
nuget restore -PackagesDirectory packages UT/packages.config

sn -k eLyKseeR.snk
sn -k eLyKseeR-native.snk
sn -k UT/ut.snk

xbuild /t:clean /p:Configuration="Debug" /p:Platform="${PLATFRM}" eLyKseeR-base.OSX.sln
xbuild /p:Configuration="Debug" /p:Platform="${PLATFRM}" eLyKseeR-base.OSX.sln

mono packages/NUnit.Runners.lite.2.6.4.20150512/nunit-console.exe -labels UT/bin/Debug/UT.exe

# NUnit test results in: TestResult.xml

