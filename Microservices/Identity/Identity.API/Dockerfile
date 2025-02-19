FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Microservices/Identity/Identity.API/Identity.API.csproj", "Microservices/Identity/Identity.API/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Microservices/Identity/Identity.DataAccess/Identity.DataAccess.csproj", "Microservices/Identity/Identity.DataAccess/"]
COPY ["Microservices/Identity/Identity.Domain/Identity.Domain.csproj", "Microservices/Identity/Identity.Domain/"]
RUN dotnet restore "Microservices/Identity/Identity.API/Identity.API.csproj"
COPY . .
WORKDIR "/src/Microservices/Identity/Identity.API"
RUN dotnet build "Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.API.dll"]
