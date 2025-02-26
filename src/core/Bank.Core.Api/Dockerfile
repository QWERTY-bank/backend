FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/core/Bank.Core.Api/Bank.Core.Api.csproj", "src/core/Bank.Core.Api/"]
COPY ["src/core/Bank.Core.Application/Bank.Core.Application.csproj", "src/core/Bank.Core.Application/"]
COPY ["src/core/Bank.Core.Domain/Bank.Core.Domain.csproj", "src/core/Bank.Core.Domain/"]
COPY ["src/common/Bank.Common.Api/Bank.Common.Api.csproj", "src/common/Bank.Common.Api/"]
RUN dotnet restore "src/core/Bank.Core.Api/Bank.Core.Api.csproj"
COPY . .
WORKDIR "/src/src/core/Bank.Core.Api"
RUN dotnet build "Bank.Core.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Bank.Core.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bank.Core.Api.dll"]
