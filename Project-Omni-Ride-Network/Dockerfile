#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Project-Omni-Ride-Network/Project-Omni-Ride-Network.csproj", "Project-Omni-Ride-Network/"]
RUN dotnet restore "Project-Omni-Ride-Network/Project-Omni-Ride-Network.csproj"
COPY . .
WORKDIR "/src/Project-Omni-Ride-Network"
RUN dotnet build "Project-Omni-Ride-Network.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Project-Omni-Ride-Network.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Project-Omni-Ride-Network.dll"]