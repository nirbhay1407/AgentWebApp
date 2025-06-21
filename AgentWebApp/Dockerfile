# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Use the official .NET 8 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["AgentWebApp/AgentWebApp.csproj", "AgentWebApp/"]
RUN dotnet restore "AgentWebApp/AgentWebApp.csproj"

# Copy everything else and build
COPY . .
WORKDIR /src/AgentWebApp
RUN dotnet publish "AgentWebApp.csproj" -c Release -o /app/publish --no-restore

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "AgentWebApp.dll"]