FROM mcr.microsoft.com/dotnet/aspnet:5.0.9-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TestBedII/TestBedII.csproj", "TestBedII/"]
RUN dotnet restore "TestBedII/TestBedII.csproj"
COPY . .
WORKDIR "/src/TestBedII"
RUN dotnet build "TestBedII.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestBedII.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestBedII.dll"]