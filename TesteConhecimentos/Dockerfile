﻿FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
EXPOSE 80
EXPOSE 443
WORKDIR /App

COPY TesteConhecimentos.csproj ./
RUN dotnet restore

COPY ../ ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "TesteConhecimentos.dll"]
