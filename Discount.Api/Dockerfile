# Use the base image with ASP.NET Core runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the SDK image to build the project files
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy csproj files for each project and restore separately to utilize Docker cache
COPY Discount.Api/Discount.Api.csproj Discount.Api/
COPY Discount.Webframework/Discount.Webframework.csproj Discount.Webframework/
COPY Discount.Infrastructure/Discount.Infrastructure.csproj Discount.Infrastructure/
COPY Discount.Application/Discount.Application.csproj Discount.Application/
COPY Discount.Core/Discount.Core.csproj Discount.Core/
COPY Discount.Common/Discount.Common.csproj Discount.Common/

# Run dotnet restore for each project
RUN dotnet restore "Discount.Api/Discount.Api.csproj"
RUN dotnet restore "Discount.Webframework/Discount.Webframework.csproj"
RUN dotnet restore "Discount.Infrastructure/Discount.Infrastructure.csproj"
RUN dotnet restore "Discount.Application/Discount.Application.csproj"
RUN dotnet restore "Discount.Core/Discount.Core.csproj"
RUN dotnet restore "Discount.Common/Discount.Common.csproj"

# Copy the rest of the application source code
COPY . .

# Set the working directory to the main project directory and build the application
WORKDIR "/src/Discount.Api"
RUN dotnet build "Discount.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Discount.Api.csproj" -c Release -o /app/publish

# Generate the runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Discount.Api.dll"]
