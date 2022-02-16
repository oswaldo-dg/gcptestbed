# FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine3.8 AS BASE
 FROM mcr.microsoft.com/dotnet/aspnet:3.1.22-alpine3.14 AS base

# ENV ASPNETCORE_VERSION=3.1.0

# RUN apk update 
# RUN apk add libc6-compat 
# RUN apk add bash 
# RUN rm -Rf /usr/share/dotnet 
# RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/3.1.0/aspnetcore-runtime-3.1.0-linux-musl-x64.tar.gz 
# RUN mkdir -p /usr/share/dotnet 
# RUN tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet && rm aspnetcore.tar.gz 
# RUN ln -sf /usr/share/dotnet/dotnet /usr/bin/dotnet

# RUN apk update 
# RUN apk upgrade 
# RUN apk add gcompat

RUN apk update --no-cache
RUn apk del libc6-compat-1.2.2-r3
RUN apk add --no-cache bash libc6-compat=1.1.19-r11

WORKDIR /app
EXPOSE 6000
EXPOSE 6001

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
