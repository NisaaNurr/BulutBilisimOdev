FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
#EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /
COPY ["Odev1/Odev1.csproj", "Odev1/"]

RUN dotnet restore "Odev1/Odev1.csproj"
COPY . .
WORKDIR "/Odev1"
RUN dotnet build "Odev1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Odev1.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# ENV ASPNETCORE_ENVIRONMENT=Development
#ENV ASPNETCORE_URLS="http://*:1453"
ENTRYPOINT ["dotnet", "Odev1.dll"]