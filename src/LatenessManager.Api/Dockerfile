FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/LatenessManager.Api/LatenessManager.Api.csproj", "src/LatenessManager.Api/"]
RUN dotnet restore "src/LatenessManager.Api/LatenessManager.Api.csproj"
COPY . .
WORKDIR "/src/src/LatenessManager.Api"
RUN dotnet build "LatenessManager.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LatenessManager.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LatenessManager.Api.dll"]
