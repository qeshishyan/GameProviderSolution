#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 7189

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/GameProviderService/GameProviderService.csproj", "src/GameProviderService/"]
RUN dotnet restore "src/GameProviderService/GameProviderService.csproj"
COPY . .
WORKDIR "/src/src/GameProviderService"
RUN dotnet build "GameProviderService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameProviderService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameProviderService.dll"]