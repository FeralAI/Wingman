#!/bin/bash

# Clear previous results
rm -rf tests/_results/*

# Find all directories that start with "Wingman."
for d in ./tests/WingmanTests.*
do
	cd $d
	dotnet test -p:CollectCoverage=true -p:CoverletOutputFormat=opencover -p:CoverletOutput="../_results/${d/.\/tests\//}.xml" -p:Exclude="[*]Tests.*" &
	cd ../..
done

wait

# Generate HTML report
cd tests/WingmanTests
dotnet restore
dotnet reportgenerator "-reports:../_results/*.xml" "-targetdir:../_results/html"
cd ../..

# Open the HTML report
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
start chrome "${DIR}/tests/_results/html/index.htm"
