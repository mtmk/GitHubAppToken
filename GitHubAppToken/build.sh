set -e 

rm -rf dist
dotnet pack -c release -o dist

if [ "$1" = "push" ]
then
  cd dist
  dotnet nuget push *.nupkg -k $(cat ~/.keys/nuget)
fi
