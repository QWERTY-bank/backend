# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/users/Bank.Users.Api/Bank.Users.Api.csproj", "src/users/Bank.Users.Api/"]
COPY ["src/common/Bank.Common.Api/Bank.Common.Api.csproj", "src/common/Bank.Common.Api/"]
COPY ["src/common/Bank.Common.Auth/Bank.Common.Auth.csproj", "src/common/Bank.Common.Auth/"]
COPY ["src/common/Bank.Common.Models/Bank.Common.Models.csproj", "src/common/Bank.Common.Models/"]
COPY ["src/users/Bank.Users.Application/Bank.Users.Application.csproj", "src/users/Bank.Users.Application/"]
COPY ["src/common/Bank.Common.Application/Bank.Common.Application.csproj", "src/common/Bank.Common.Application/"]
COPY ["src/users/Bank.Users.Persistence/Bank.Users.Persistence.csproj", "src/users/Bank.Users.Persistence/"]
COPY ["src/users/Bank.Users.Domain/Bank.Users.Domain.csproj", "src/users/Bank.Users.Domain/"]
RUN dotnet restore "./src/users/Bank.Users.Api/Bank.Users.Api.csproj"
COPY . .
WORKDIR "/src/src/users/Bank.Users.Api"
RUN dotnet build "./Bank.Users.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Bank.Users.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bank.Users.Api.dll"]