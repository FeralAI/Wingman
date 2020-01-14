#!/bin/bash

# dotnet tool install -g dotnet-reportgenerator-globaltool

cd NETStandardLibrary.Common.Tests
dotnet test -p:CollectCoverage=true -p:CoverletOutputFormat=opencover -p:CoverletOutput="../TestResults/NETStandardLibrary.Common.Tests.xml" -p:Exclude="[*]Tests.*"
cd ..

cd NETStandardLibrary.Email.Tests
dotnet test -p:CollectCoverage=true -p:CoverletOutputFormat=opencover -p:CoverletOutput="../TestResults/NETStandardLibrary.Email.Tests.xml" -p:Exclude="[*]Tests.*"
cd ..

cd NETStandardLibrary.Linq.Tests
dotnet test -p:CollectCoverage=true -p:CoverletOutputFormat=opencover -p:CoverletOutput="../TestResults/NETStandardLibrary.Linq.Tests.xml" -p:Exclude="[*]Tests.*"
# dotnet reportgenerator "-reports:../TestResults/*.xml" "-targetdir:../TestResults/html"
cd ..

# cd NETStandardLibrary.Search.Tests
# dotnet test -p:CollectCoverage=true -p:CoverletOutputFormat=opencover -p:CoverletOutput="../TestResults/NETStandardLibrary.Search.Tests.xml" -p:Exclude="[*]Tests.*"
# dotnet reportgenerator "-reports:../TestResults/*.xml" "-targetdir:../TestResults/html"
# cd ..

cd NETStandardLibrary.Common.Tests
dotnet reportgenerator "-reports:../TestResults/*.xml" "-targetdir:../TestResults/html"
cd ..

# dotnet reportgenerator "-reports:TestResults/*.xml" "-targetdir:TestResults/html"
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
start chrome "${DIR}/TestResults/html/index.htm"
