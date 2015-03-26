msbuild muffin.csproj /p:Configuration=Release
..\.nuget\NuGet pack Muffin.nuspec
..\.nuget\NuGet push Muffin.%1.nupkg -s http://nuget.ismarvelous.nl/ marvelou$