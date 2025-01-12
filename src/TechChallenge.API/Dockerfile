FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000
EXPOSE 5001


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TechChallenge.API/TechChallenge.API.csproj", "src/TechChallenge.API/"]
COPY ["src/TechChallenge.Application/TechChallenge.Application.csproj", "src/TechChallenge.Application/"]
COPY ["src/TechChallenge.Domain/TechChallenge.Domain.csproj", "src/TechChallenge.Domain/"]
COPY ["src/TechChallenge.Infrastructure/TechChallenge.Infrastructure.csproj", "src/TechChallenge.Infrastructure/"]
RUN dotnet restore "./src/TechChallenge.API/TechChallenge.API.csproj"
COPY . .
WORKDIR "/src/src/TechChallenge.API"
RUN dotnet build "./TechChallenge.API.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TechChallenge.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
RUN mkdir -p /app/logs && chmod -R 777 /app


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechChallenge.API.dll"]