FROM microsoft/dotnet:2.2-sdk-alpine AS build

WORKDIR /sln

# copy csproj and restore as distinct layers
COPY *.sln .

COPY Trivia.API/*.csproj ./Trivia.API/
COPY Trivia.Loader/*.csproj ./Trivia.Loader/

RUN dotnet restore

# copy everything else and build app
COPY . .

RUN dotnet build -c Release
# RUN dotnet test 
RUN dotnet publish -c Release -o ../dist

FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine AS runtime

WORKDIR /app

COPY --from=build /sln/dist .

ENTRYPOINT ["dotnet", "Trivia.API.dll"]