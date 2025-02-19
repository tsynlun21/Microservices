FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Microservices/CarHistory/CarHistory.API/CarHistory.API.csproj", "Microservices/CarHistory/CarHistory.API/"]
COPY ["Microservices/CarHistory/CarHistory.DataAccess/CarHistory.DataAccess.csproj", "Microservices/CarHistory/CarHistory.DataAccess/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Microservices/CarHistory/CarHistory.Domain/CarHistory.Domain.csproj", "Microservices/CarHistory/CarHistory.Domain/"]
RUN dotnet restore "Microservices/CarHistory/CarHistory.API/CarHistory.API.csproj"
COPY . .
WORKDIR "/src/Microservices/CarHistory/CarHistory.API"
RUN dotnet build "CarHistory.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CarHistory.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarHistory.API.dll"]
