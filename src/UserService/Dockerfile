FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5158

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["UserService/UserService.csproj", "UserService/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["Shared/", "Shared/"]

RUN dotnet restore "UserService/UserService.csproj"

WORKDIR "/src/Shared"
RUN dotnet build "Shared.csproj" -c Release

WORKDIR "/src/UserService.Domain"
RUN dotnet build "UserService.Domain.csproj" -c Release

WORKDIR "/src/UserService.Infrastructure"
RUN dotnet build "UserService.Infrastructure.csproj" -c Release

WORKDIR "/src/UserService"
COPY UserService/ .
RUN dotnet build "UserService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserService.dll"]