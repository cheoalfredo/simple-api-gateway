#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["K8SCore/K8SCore.csproj", "K8SCore/"]
RUN dotnet restore "K8SCore/K8SCore.csproj"
COPY . .
WORKDIR "/src/K8SCore"
RUN dotnet build "K8SCore.csproj" -c Release -o /app/build

FROM build AS publish
RUN "pwd"
RUN dotnet publish "K8SCore.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish  /src/K8SCore/Templates ./Templates
ENTRYPOINT ["dotnet", "K8SCore.dll"]