FROM mcr.microsoft.com/dotnet/core/sdk:2.2 as builder

COPY ./src/NotificationUI ./app/

WORKDIR /app

RUN dotnet build NotificationUI.csproj
