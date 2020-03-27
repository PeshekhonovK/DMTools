FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY IdentityServer/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY ./IdentityServer ./
RUN dotnet publish -c Debug -o out --no-self-contained -r linux-x64

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
WORKDIR /app
COPY --from=build-env /app/out .
ENV ASPNETCORE_URLS="https://+"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnet.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password="123qwe!E"
EXPOSE 433
ENTRYPOINT ["dotnet", "IdentityServer.dll", "--environment=Development"]
