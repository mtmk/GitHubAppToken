set -e

rm -rf dist

dotnet publish -c release -r osx-x64 -p:PublishTrimmed=true -o dist/osx-x64 --sc -p:PublishSingleFile=true -p:DebugType=embedded
dotnet publish -c release -r linux-x64 -p:PublishTrimmed=true -o dist/linux-x64 --sc -p:PublishSingleFile=true -p:DebugType=embedded
dotnet publish -c release -r win-x64 -p:PublishTrimmed=true -o dist/win-x64 --sc -p:PublishSingleFile=true -p:DebugType=embedded

pushd dist/osx-x64
zip -r ../ghtoken-osx-x64.zip ./*
popd

pushd dist/linux-x64
zip -r ../ghtoken-linux-x64.zip ./*
popd

pushd dist/win-x64
zip -r ../ghtoken-win-x64.zip ./*
popd
