# Use the official Alpine-based image as a parent image
FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build-env

# Define build arguments and environment variables
ARG BUILD_CONFIGURATION=Debug
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
ENV ASPNETCORE_URLS=http://+:80

# Expose port 80 for the application to listen on
EXPOSE 80

# Set the working directory to /src
WORKDIR /src

# Copy project files for restoration
COPY Catalog.Api/Catalog.Api.csproj Catalog.Api/
COPY Catalog.WebFramework/Catalog.WebFramework.csproj Catalog.WebFramework/
COPY Catalog.Infrastructure/Catalog.Infrastructure.csproj Catalog.Infrastructure/
COPY Catalog.Application/Catalog.Application.csproj Catalog.Application/
COPY Catalog.Core/Catalog.Core.csproj Catalog.Core/
COPY Catalog.Common/Catalog.Common.csproj Catalog.Common/

# Restore all projects using 'dotnet restore'
RUN dotnet restore "Catalog.Api/Catalog.Api.csproj"
RUN dotnet restore "Catalog.WebFramework/Catalog.WebFramework.csproj"
RUN dotnet restore "Catalog.Infrastructure/Catalog.Infrastructure.csproj"
RUN dotnet restore "Catalog.Application/Catalog.Application.csproj"
RUN dotnet restore "Catalog.Core/Catalog.Core.csproj"
RUN dotnet restore "Catalog.Common/Catalog.Common.csproj"

# Copy the rest of the application source code
COPY . .

# Set the working directory to the Catalog.Api project
WORKDIR "/src/Catalog.Api"

# Build the application in Release mode and output to /app/build
RUN dotnet build "Catalog.Api.csproj" -c Release -o /app/build

# Specify the entry point and command to run the application
ENTRYPOINT ["dotnet", "/app/build/Catalog.Api.dll"]
CMD ["--server.urls", "http://0.0.0.0:80"]
