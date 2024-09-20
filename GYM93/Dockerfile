FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["GYM93.csproj", "./"]
RUN dotnet restore "GYM93.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "GYM93.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GYM93.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GYM93.dll"]
