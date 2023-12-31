FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 7189

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["GameProviderService/GameProviderService.csproj", "GameProviderService/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["Shared/", "Shared/"]

RUN dotnet restore "GameProviderService/GameProviderService.csproj"

WORKDIR "/src/Shared"
RUN dotnet build "Shared.csproj" -c Release

WORKDIR "/src/GameProviderService"
COPY GameProviderService/ .
RUN dotnet build "GameProviderService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameProviderService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameProviderService.dll"]