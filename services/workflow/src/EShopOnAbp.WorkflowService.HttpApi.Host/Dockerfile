# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["services/workflow/src/EShopOnAbp.WorkflowService.HttpApi.Host/EShopOnAbp.WorkflowService.HttpApi.Host.csproj", "services/workflow/src/EShopOnAbp.WorkflowService.HttpApi.Host/"]
COPY ["aspire/ServiceDefaults/eShopOnAbp.ServiceDefaults.csproj", "aspire/ServiceDefaults/"]
COPY ["shared/EShopOnAbp.Shared.Hosting.AspNetCore/EShopOnAbp.Shared.Hosting.AspNetCore.csproj", "shared/EShopOnAbp.Shared.Hosting.AspNetCore/"]
COPY ["shared/EShopOnAbp.Shared.Hosting/EShopOnAbp.Shared.Hosting.csproj", "shared/EShopOnAbp.Shared.Hosting/"]
COPY ["shared/EShopOnAbp.Shared.Localization/EShopOnAbp.Shared.Localization.csproj", "shared/EShopOnAbp.Shared.Localization/"]
COPY ["shared/EShopOnAbp.Shared.Hosting.Gateways/EShopOnAbp.Shared.Hosting.Gateways.csproj", "shared/EShopOnAbp.Shared.Hosting.Gateways/"]
COPY ["shared/EShopOnAbp.Shared.Hosting.Microservices/EShopOnAbp.Shared.Hosting.Microservices.csproj", "shared/EShopOnAbp.Shared.Hosting.Microservices/"]
RUN dotnet restore "./services/workflow/src/EShopOnAbp.WorkflowService.HttpApi.Host/EShopOnAbp.WorkflowService.HttpApi.Host.csproj"
COPY . .
WORKDIR "/src/services/workflow/src/EShopOnAbp.WorkflowService.HttpApi.Host"
RUN dotnet build "./EShopOnAbp.WorkflowService.HttpApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EShopOnAbp.WorkflowService.HttpApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShopOnAbp.WorkflowService.HttpApi.Host.dll"]
