#!/usr/bin/env bash
function build() {
  dotnet build -c Release src/Covid19Api.Mongo.Migrator/Covid19Api.Mongo.Migrator.csproj -o publish 
}


function execute() {
  dotnet ./publish/Covid19Api.Mongo.Migrator.dll
}

build
execute
exit ${?}