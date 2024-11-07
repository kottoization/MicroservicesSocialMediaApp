
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./IdentityAPI.csproj" --disable-parallel
RUN dotnet publish "./IdentityAPI.csproj" -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000
ENTRYPOINT ["dotnet", "IdentityAPI.dll"]