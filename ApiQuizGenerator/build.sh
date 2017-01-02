set -ev
dotnet restore
dotnet build ./ApiQuizGenerator/src/ApiQuizGenerator/project.json
dotnet build ./ApiQuizGenerator/test/ApiQuizGenerator.Tests/project.json
dotnet test ./ApiQuizGenerator/test/ApiQuizGenerator.Tests/project.json

