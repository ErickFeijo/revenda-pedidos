# Fase base: imagem runtime do .NET 8
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Fase build: compila o projeto usando SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/RevendaPedidos.Worker/RevendaPedidos.Worker.csproj", "src/RevendaPedidos.Worker/"]
COPY ["src/RevendaPedidos.Application/RevendaPedidos.Application.csproj", "src/RevendaPedidos.Application/"]
COPY ["src/RevendaPedidos.Domain/RevendaPedidos.Domain.csproj", "src/RevendaPedidos.Domain/"]
COPY ["src/RevendaPedidos.Shared/RevendaPedidos.Shared.csproj", "src/RevendaPedidos.Shared/"]
COPY ["src/RevendaPedidos.DI/RevendaPedidos.DI.csproj", "src/RevendaPedidos.DI/"]
COPY ["src/RevendaPedidos.Application.Impl/RevendaPedidos.Application.Impl.csproj", "src/RevendaPedidos.Application.Impl/"]
COPY ["src/RevendaPedidos.Infrastructure/RevendaPedidos.Infrastructure.csproj", "src/RevendaPedidos.Infrastructure/"]
RUN dotnet restore "./src/RevendaPedidos.Worker/RevendaPedidos.Worker.csproj"
COPY . .
WORKDIR "/src/src/RevendaPedidos.Worker"
RUN dotnet build "./RevendaPedidos.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build # Fase publish: publica apenas os arquivos necessários para execução FROM build AS publish ARG BUILD_CONFIGURATION=Release RUN dotnet publish "./RevendaPedidos.Worker.csproj" -c$BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Fase final: cria imagem enxuta apenas com o publicado
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RevendaPedidos.Worker.dll"]
