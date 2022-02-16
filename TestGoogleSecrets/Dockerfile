FROM mcr.microsoft.com/dotnet/aspnet:3.1.22-alpine3.14 AS base
RUN apk update 
RUN apk upgrade 

WORKDIR /app
EXPOSE 6000
EXPOSE 6001

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["TestGoogleSecrets/TestGoogleSecrets.csproj", "TestGoogleSecrets/"]
RUN dotnet restore "TestGoogleSecrets/TestGoogleSecrets.csproj"
COPY . .
WORKDIR "/src/TestGoogleSecrets"
RUN dotnet build "TestGoogleSecrets.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestGoogleSecrets.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestGoogleSecrets.dll"]