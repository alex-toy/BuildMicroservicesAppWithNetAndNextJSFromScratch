#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BiddingService/BiddingService.csproj", "BiddingService/"]
COPY ["Contracts/Contracts.csproj", "Contracts/"]
RUN dotnet restore "BiddingService/BiddingService.csproj"
COPY . .
WORKDIR "/src/BiddingService"
RUN dotnet build "BiddingService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BiddingService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BiddingService.dll"]