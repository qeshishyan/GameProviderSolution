FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 7021

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["CrashGameService/CrashGameService.csproj", "CrashGameService/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["Shared/", "Shared/"]

RUN dotnet restore "CrashGameService/CrashGameService.csproj"

WORKDIR "/src/Shared"
RUN dotnet build "Shared.csproj" -c Release

WORKDIR "/src/CrashGameService"
COPY CrashGameService/ .
RUN dotnet build "CrashGameService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CrashGameService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CrashGameService.dll"]

