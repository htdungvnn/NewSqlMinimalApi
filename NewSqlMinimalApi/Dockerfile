# Use the official ASP.NET runtime image as base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# Copy csproj(s) and restore dependencies
COPY NewSqlMinimalApi/NewSqlMinimalApi.csproj NewSqlMinimalApi/
RUN dotnet restore NewSqlMinimalApi/NewSqlMinimalApi.csproj

# Copy the entire source code
COPY . .

# Build the project
WORKDIR /src/NewSqlMinimalApi
RUN dotnet build NewSqlMinimalApi.csproj -c $BUILD_CONFIGURATION -o /app/build

# Publish the app
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish NewSqlMinimalApi.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "NewSqlMinimalApi.dll"]