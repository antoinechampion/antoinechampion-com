FROM mcr.microsoft.com/dotnet/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1-buster AS build
WORKDIR /src
COPY . .
RUN dotnet restore "antoinechampion-com.csproj" 
RUN dotnet build "antoinechampion-com.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "antoinechampion-com.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "antoinechampion-com.dll"]