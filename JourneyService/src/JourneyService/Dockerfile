FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["JourneyService.csproj", "./"]
RUN dotnet restore "./JourneyService.csproj"

# Copy everything else and build
COPY . ./
RUN dotnet build "JourneyService.csproj" -c Release -o /app/build

FROM build-env AS publish
RUN dotnet publish "JourneyService.csproj" -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=publish /app/out .

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

ENTRYPOINT ["dotnet", "/app/JourneyService.dll"]
