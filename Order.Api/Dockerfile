# Build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["Order.Api/Order.Api.csproj", "Order.Api/"]
COPY ["Order.Webframework/Order.Webframework.csproj", "Order.Webframework/"]
COPY ["Order.Infrastructure/Order.Infrastructure.csproj", "Order.Infrastructure/"]
COPY ["Order.Application/Order.Application.csproj", "Order.Application/"]
COPY ["Order.Core/Order.Core.csproj", "Order.Core/"]
COPY ["Order.Common/Order.Common.csproj", "Order.Common/"]

RUN dotnet restore "Order.Api/Order.Api.csproj"
COPY . .
WORKDIR "/src/Order.Api"
RUN dotnet build "Order.Api.csproj" -c Release -o /app/build

# Publish stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS publish
WORKDIR /app
COPY --from=build /app/build ./

# Final stage
FROM publish AS final
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "Order.Api.dll"]
