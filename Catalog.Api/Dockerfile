# Use the official image as a parent image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /src

# Copy the csproj files
COPY Catalog.Api/Catalog.Api.csproj Catalog.Api/
COPY Catalog.WebFramework/Catalog.WebFramework.csproj Catalog.WebFramework/
COPY Catalog.Infrastructure/Catalog.Infrastructure.csproj Catalog.Infrastructure/
COPY Catalog.Application/Catalog.Application.csproj Catalog.Application/
COPY Catalog.Core/Catalog.Core.csproj Catalog.Core/
COPY Catalog.Common/Catalog.Common.csproj Catalog.Common/

# Restore all projects
RUN dotnet restore "Catalog.Api/Catalog.Api.csproj"
RUN dotnet restore "Catalog.WebFramework/Catalog.WebFramework.csproj"
RUN dotnet restore "Catalog.Infrastructure/Catalog.Infrastructure.csproj"
RUN dotnet restore "Catalog.Application/Catalog.Application.csproj"
RUN dotnet restore "Catalog.Core/Catalog.Core.csproj"
RUN dotnet restore "Catalog.Common/Catalog.Common.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/Catalog.Api"
RUN dotnet build "Catalog.Api.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/build .
ENTRYPOINT ["dotnet", "Catalog.Api.dll"]
