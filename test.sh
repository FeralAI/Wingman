#!/bin/bash

# dotnet tool install -g dotnet-reportgenerator-globaltool

cd tests/NETStandardLibraryTests.Common
dotnet test -p:CollectCoverage=true -p:CoverletOutputFormat=opencover -p:CoverletOutput="../_results/NETStandardLibraryTests.Common.xml" -p:Exclude="[*]Tests.*"
cd ../..

cd tests/NETStandardLibraryTests.Email
dotnet test -p:CollectCoverage=true -p:CoverletOutputFormat=opencover -p:CoverletOutput="../_results/NETStandardLibraryTests.Email.xml" -p:Exclude="[*]Tests.*"
cd ../..

cd tests/NETStandardLibraryTests.Linq
dotnet test -p:CollectCoverage=true -p:CoverletOutputFormat=opencover -p:CoverletOutput="../_results/NETStandardLibraryTests.Linq.xml" -p:Exclude="[*]Tests.*"
cd ../..

cd tests/NETStandardLibraryTests.Search
dotnet test -p:CollectCoverage=true -p:CoverletOutputFormat=opencover -p:CoverletOutput="../_results/NETStandardLibraryTests.Search.xml" -p:Exclude="[*]Tests.*"
cd ../..

cd tests/NETStandardLibraryTests
dotnet restore
dotnet reportgenerator "-reports:../_results/*.xml" "-targetdir:../_results/html"
cd ../..

# dotnet reportgenerator "-reports:_results/*.xml" "-targetdir:_results/html"
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
start chrome "${DIR}/tests/_results/html/index.htm"
