# Use the official image as a parent image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

# Define build arguments and environment variables
ARG BUILD_CONFIGURATION=Debug
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
ENV ASPNETCORE_URLS=http://+:80

# Expose port 80 for the application to listen on
EXPOSE 80

# Set the working directory to /src
WORKDIR /src

COPY Basket.Api/Basket.Api.csproj Basket.Api/
COPY Basket.WebFramework/Basket.WebFramework.csproj Basket.WebFramework/
COPY Basket.Infrastructure/Basket.Infrastructure.csproj Basket.Infrastructure/
COPY Basket.Application/Basket.Application.csproj Basket.Application/
COPY Basket.Core/Basket.Core.csproj Basket.Core/
COPY Basket.Common/Basket.Common.csproj Basket.Common/

# Run dotnet restore for each project
RUN dotnet restore "Basket.Api/Basket.Api.csproj"
RUN dotnet restore "Basket.WebFramework/Basket.WebFramework.csproj"
RUN dotnet restore "Basket.Infrastructure/Basket.Infrastructure.csproj"
RUN dotnet restore "Basket.Application/Basket.Application.csproj"
RUN dotnet restore "Basket.Core/Basket.Core.csproj"
RUN dotnet restore "Basket.Common/Basket.Common.csproj"

# Copy the rest of the application source code
COPY . .

WORKDIR "/src/Basket.Api"
RUN dotnet build "Basket.Api.csproj" -c Release -o /app/build

ENTRYPOINT ["dotnet", "/app/build/Basket.Api.dll"]
CMD ["--server.urls", "http://0.0.0.0:80"]
