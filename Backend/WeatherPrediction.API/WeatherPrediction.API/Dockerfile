# Use official .NET runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8080

# Build image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WeatherApi.csproj", "./"]
RUN dotnet restore "./WeatherApi.csproj"
COPY . .
RUN dotnet build "WeatherApi.csproj" -c Release -o /app/build

# Publish image
FROM build AS publish
RUN dotnet publish "WeatherApi.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherApi.dll"]
