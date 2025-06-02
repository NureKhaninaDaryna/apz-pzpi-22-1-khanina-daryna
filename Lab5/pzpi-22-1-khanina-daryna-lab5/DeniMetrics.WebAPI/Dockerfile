FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_14.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY ["DeniMetrics.WebAPI/DeniMetrics.WebAPI.csproj", "DeniMetrics.WebAPI/"]
COPY ["DineMetrics.BLL/DineMetrics.BLL.csproj", "DineMetrics.BLL/"]
COPY ["DineMetrics.Core/DineMetrics.Core.csproj", "DineMetrics.Core/"]
COPY ["DineMetrics.DAL/DineMetrics.DAL.csproj", "DineMetrics.DAL/"]
RUN dotnet restore "DeniMetrics.WebAPI/DeniMetrics.WebAPI.csproj"
COPY . .
WORKDIR "/src/DeniMetrics.WebAPI"
RUN dotnet build "DeniMetrics.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DeniMetrics.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeniMetrics.WebAPI.dll"]


