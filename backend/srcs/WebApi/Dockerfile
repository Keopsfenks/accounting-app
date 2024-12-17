FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["../srcs/WebApi/WebApi.csproj", "../srcs/WebApi/"]
COPY ["../srcs/core/Application/Application.csproj", "../srcs/core/Application/"]
COPY ["../srcs/core/Domain/Domain.csproj", "../srcs/core/Domain/"]
COPY ["../srcs/external/Infrastructure/Infrastructure.csproj", "../srcs/external/Infrastructure/"]
COPY ["../srcs/external/Persistance/Persistance.csproj", "../srcs/external/Persistance/"]
RUN dotnet restore "../srcs/WebApi/WebApi.csproj"
COPY . .
WORKDIR "../srcs/WebApi"
RUN dotnet build "WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]
