# Use a base image that is compatible with .NET 7.0 and gRPC
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Define build arguments and environment variables
ARG BUILD_CONFIGURATION=Release
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true

COPY Discount.Api/Discount.Api.csproj Discount.Api/
COPY Discount.Webframework/Discount.Webframework.csproj Discount.Webframework/
COPY Discount.Infrastructure/Discount.Infrastructure.csproj Discount.Infrastructure/
COPY Discount.Application/Discount.Application.csproj Discount.Application/
COPY Discount.Core/Discount.Core.csproj Discount.Core/
COPY Discount.Common/Discount.Common.csproj Discount.Common/

# Run dotnet restore for each project
RUN dotnet restore "Discount.Api/Discount.Api.csproj" \
 && dotnet restore "Discount.Webframework/Discount.Webframework.csproj" \
 && dotnet restore "Discount.Infrastructure/Discount.Infrastructure.csproj" \
 && dotnet restore "Discount.Application/Discount.Application.csproj" \
 && dotnet restore "Discount.Core/Discount.Core.csproj" \
 && dotnet restore "Discount.Common/Discount.Common.csproj"

# Copy the rest of the application source code
COPY . .

WORKDIR "/src/Discount.Api"
RUN dotnet build "Discount.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM base AS final
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "Discount.Api.dll"]
