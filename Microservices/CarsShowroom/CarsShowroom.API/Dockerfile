FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Microservices/CarsShowroom/CarsShowroom.API/CarsShowroom.API.csproj", "Microservices/CarsShowroom/CarsShowroom.API/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Microservices/CarsShowroom/CarsShowroom.DataAccess/CarsShowroom.DataAccess.csproj", "Microservices/CarsShowroom/CarsShowroom.DataAccess/"]
COPY ["Microservices/CarsShowroom/CarsShowroom.Domain/CarsShowroom.Domain.csproj", "Microservices/CarsShowroom/CarsShowroom.Domain/"]
RUN dotnet restore "Microservices/CarsShowroom/CarsShowroom.API/CarsShowroom.API.csproj"
COPY . .
WORKDIR "/src/Microservices/CarsShowroom/CarsShowroom.API"
RUN dotnet build "CarsShowroom.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CarsShowroom.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarsShowroom.API.dll"]
