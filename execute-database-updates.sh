#!/usr/bin/env bash
function build() {
  dotnet build -c Release src/Covid19Api.Mongo.Scaffolder/Covid19Api.Mongo.Scaffolder.csproj -o publish 
}


function execute() {
  dotnet ./publish/Covid19Api.Mongo.Scaffolder.dll
}

build
execute
exit ${?}