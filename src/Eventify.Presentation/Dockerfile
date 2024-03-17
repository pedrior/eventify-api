FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Eventify.Presentation/Eventify.Presentation.csproj", "src/Eventify.Presentation/"]
COPY ["src/Eventify.Application/Eventify.Application.csproj", "src/Eventify.Application/"]
COPY ["src/Eventify.Domain/Eventify.Domain.csproj", "src/Eventify.Domain/"]
COPY ["src/Eventify.Infrastructure/Eventify.Infrastructure.csproj", "src/Eventify.Infrastructure/"]
RUN dotnet restore "src/Eventify.Presentation/Eventify.Presentation.csproj"
COPY . .
WORKDIR "/src/src/Eventify.Presentation"
RUN dotnet build "Eventify.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Eventify.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Eventify.Presentation.dll"]
