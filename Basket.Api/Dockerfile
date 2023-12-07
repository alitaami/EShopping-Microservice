# See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Build stage
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

# Publish stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS publish
WORKDIR /app
COPY --from=build /app/build ./

# Final stage
FROM publish AS final
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "Basket.Api.dll"]
