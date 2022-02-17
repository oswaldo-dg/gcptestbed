# FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine3.8 AS BASE
 # FROM mcr.microsoft.com/dotnet/aspnet:3.1.9-alpine3.12 AS base
FROM marketplace.gcr.io/google/debian11 AS base

# RUN apt-get update
# RUN apt-get install -y wget

# RUN wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
# RUN dpkg -i packages-microsoft-prod.deb
# RUN rm packages-microsoft-prod.deb

# RUN apt-get install -y apt-transport-https
# RUN apt-get update
# RUN apt-get install -y aspnetcore-runtime-3.1

# RUN apt-get -y --purge remove wget

# # ENV ASPNETCORE_VERSION=3.1.22
# RUN mkdir /usr/share/dotnet
# RUN curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/3.1.22/aspnetcore-runtime-3.1.22-linux-x64.tar.gz 
# RUN aspnetcore_sha512='32332a09a2f05e56ca66fff2a27e988dc35dc12dcc312b62c99c2a7b2532fb02bab0fa7f49f73938999488c0463a0c79c151b020e01104b8e9cfab80a877b5f3' 
# # RUN echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - 
# RUN tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App 
# RUN rm aspnetcore.tar.gz

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

# RUN apk update --no-cache
# RUn apk del libc6-compat
# RUN apk add --no-cache bash libc6-compat=1.1.19-r11

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
