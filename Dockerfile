FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR "/build"

COPY PanpsBot.sln .
COPY PanpsBot/PanpsBot.csproj PanpsBot/
COPY PanpsBot.Functions/PanpsBot.Functions.csproj PanpsBot.Functions/
COPY PanpsBot.Infrastructure/PanpsBot.Infrastructure.csproj PanpsBot.Infrastructure/
COPY PanpsBot.Models/PanpsBot.Models.csproj PanpsBot.Models/
COPY PanpsBot.Services/PanpsBot.Services.csproj PanpsBot.Services/

RUN dotnet restore

COPY . .
RUN dotnet publish --no-restore --configuration Release --output /app PanpsBot/PanpsBot.csproj


FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app
COPY --from=build-env /app .

ENTRYPOINT ["dotnet", "PanpsBot.dll"]

