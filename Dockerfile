FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build


# copy csproj and restore as distinct layers

COPY ./src/NotificationUI/ ./app/
RUN dotnet restore ./app/NotificationUI.csproj

WORKDIR /app
RUN dotnet publish -c Release -o out
RUN ls ./out/

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/out/ ./
ENTRYPOINT ["dotnet", "NotificationUI.dll"]
