#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV DB_HOST="YourAddressdatabase"
ENV LOGIN_DB="YourLogin"
ENV PASSWORD_DB="YourPassword"
ENV DB_NAME="YourDbName"

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["HR-Helpers/HR-Helpers.csproj", "HR-Helpers/"]
COPY ["AccessData/AccessData.csproj", "AccessData/"]
RUN dotnet restore "HR-Helpers/HR-Helpers.csproj"
COPY . .
WORKDIR "/src/HR-Helpers"
RUN dotnet build "HR-Helpers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HR-Helpers.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HR-Helpers.dll"]