# See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["Discount.Api/Discount.Api.csproj", "Discount.Api/"]
COPY ["Discount.Webframework/Discount.Webframework.csproj", "Discount.Webframework/"]
COPY ["Discount.Infrastructure/Discount.Infrastructure.csproj", "Discount.Infrastructure/"]
COPY ["Discount.Application/Discount.Application.csproj", "Discount.Application/"]
COPY ["Discount.Core/Discount.Core.csproj", "Discount.Core/"]
COPY ["Discount.Common/Discount.Common.csproj", "Discount.Common/"]

RUN dotnet restore "Discount.Api/Discount.Api.csproj"
COPY . .
WORKDIR "/src/Discount.Api"
RUN dotnet build "Discount.Api.csproj" -c Release -o /app/build

# Publish stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS publish
WORKDIR /app
COPY --from=build /app/build ./

# Final stage
FROM publish AS final
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "Discount.Api.dll"]
