# See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["Catalog.Api/NuGet.Config", "Catalog.Api/"]
COPY ["Catalog.Api/Catalog.Api.csproj", "Catalog.Api/"]
COPY ["Catalog.WebFramework/Catalog.WebFramework.csproj", "Catalog.WebFramework/"]
COPY ["Catalog.Infrastructure/Catalog.Infrastructure.csproj", "Catalog.Infrastructure/"]
COPY ["Catalog.Application/Catalog.Application.csproj", "Catalog.Application/"]
COPY ["Catalog.Core/Catalog.Core.csproj", "Catalog.Core/"]
COPY ["Catalog.Common/Catalog.Common.csproj", "Catalog.Common/"]

RUN dotnet restore "Catalog.Api/Catalog.Api.csproj"
COPY . .
WORKDIR "/src/Catalog.Api"
RUN dotnet build "Catalog.Api.csproj" -c Release -o /app/build

# Publish stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS publish
WORKDIR /app
COPY --from=build /app/build ./

# Final stage
FROM publish AS final
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "Catalog.Api.dll"]
