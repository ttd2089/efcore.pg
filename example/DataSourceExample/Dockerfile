#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["NuGet.config", "."]
COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]
COPY ["example/DataSourceExample/DataSourceExample.csproj", "example/DataSourceExample/"]
COPY ["src/EFCore.PG/EFCore.PG.csproj", "src/EFCore.PG/"]
RUN dotnet restore "example/DataSourceExample/DataSourceExample.csproj"
COPY . .
WORKDIR "/src/example/DataSourceExample"
RUN dotnet build "DataSourceExample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataSourceExample.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataSourceExample.dll"]
