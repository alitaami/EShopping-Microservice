#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Basket.Api/NuGet.config", "Basket.Api/"]
COPY ["Basket.Api/Basket.Api.csproj", "Basket.Api/"]
COPY ["Basket.WebFramework/Basket.WebFramework.csproj", "Basket.WebFramework/"]
COPY ["Basket.Infrastructure/Basket.Infrastructure.csproj", "Basket.Infrastructure/"]
COPY ["Basket.Application/Basket.Application.csproj", "Basket.Application/"]
COPY ["Basket.Core/Basket.Core.csproj", "Basket.Core/"]
COPY ["Basket.Common/Basket.Common.csproj", "Basket.Common/"]
RUN dotnet restore "Basket.Api/Basket.Api.csproj"
COPY . .
WORKDIR "/src/Basket.Api"
RUN dotnet build "Basket.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Basket.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basket.Api.dll"]