#!/bin/bash

# Clear previous results
rm -rf tests/_results/*

# Find all directories that start with "NETStandardLibrary."
for d in ./tests/NETStandardLibraryTests.*
do
	cd $d
	dotnet test -p:CollectCoverage=true -p:CoverletOutputFormat=opencover -p:CoverletOutput="../_results/${d/.\/tests\//}.Common.xml" -p:Exclude="[*]Tests.*"
	cd ../..
done

# Generate HTML report
cd tests/NETStandardLibraryTests
dotnet restore
dotnet reportgenerator "-reports:../_results/*.xml" "-targetdir:../_results/html"
cd ../..

# Open the HTML report
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
start chrome "${DIR}/tests/_results/html/index.htm"
