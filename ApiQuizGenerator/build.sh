set -ev
dotnet restore ./ApiQuizGenerator/src/ApiQuizGenerator/ApiQuizGenerator.csproj
dotnet build ./ApiQuizGenerator/src/ApiQuizGenerator/ApiQuizGenerator.csproj
#dotnet test ./ApiQuizGenerator/test/ApiQuizGenerator.Tests/project.json

