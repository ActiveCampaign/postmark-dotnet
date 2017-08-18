#!/usr/bin/env bash
set -e
set -x
dir="$( dirname "$0" )/../src/"
pushd $dir
dotnet restore
msbuild /p:Configuration=Release /p:LibraryFrameworks="netstandard1.2"
dotnet test ./Postmark.Tests/Postmark.Tests.csproj /p:LibraryFrameworks='netstandard1.2'
msbuild /p:Configuration=Release /p:LibraryFrameworks="net20"
popd