FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ImagineHubAPI.csproj", "./"]
RUN dotnet restore "ImagineHubAPI.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "ImagineHubAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ImagineHubAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Copy the key files to the app directory
COPY ["public.key", "/app/public.key"]
COPY ["private.key", "/app/private.key"]
# Copy the entire wwwroot directory with its contents
COPY ["wwwroot", "/app/wwwroot"]
ENTRYPOINT ["dotnet", "ImagineHubAPI.dll"]
