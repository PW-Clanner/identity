FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Pw.Clanner.Identity.Web/Pw.Clanner.Identity.Web.csproj", "src/Pw.Clanner.Identity.Web/"]
RUN dotnet restore "src/Pw.Clanner.Identity.Web/Pw.Clanner.Identity.Web.csproj"
COPY . .
WORKDIR "/src/src/Pw.Clanner.Identity.Web"
RUN dotnet build "Pw.Clanner.Identity.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Pw.Clanner.Identity.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pw.Clanner.Identity.Web.dll"]
