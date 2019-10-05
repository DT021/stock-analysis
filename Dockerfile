FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-alpine3.9 AS build-env

RUN apk add --no-cache -U \
    nodejs \
    npm

RUN mkdir -p /app
COPY . /app
WORKDIR /app

RUN dotnet build -c Release
RUN dotnet publish ./src/web -c Release -o ../out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-alpine3.9
WORKDIR /app
COPY --from=build-env /app/out /app
ENTRYPOINT ["dotnet", "web.dll"]