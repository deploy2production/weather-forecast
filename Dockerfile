FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Copy everything
COPY . ./

# Build and publish a release
RUN dotnet publish DeployToProduction.WeatherForecast.App --configuration Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
EXPOSE 80

COPY --from=build /out .

# Set Environment Variables for the App
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_PRINT_TELEMETRY_MESSAGE=false
ENV ConnectionStrings__Postgres="Host=weather-forecast-postgres;Username=postgres;Password=postgres;Database=postgres"
ENV ConnectionStrings__Redis="weather-forecast-redis:6379"

# Start App
ENTRYPOINT ["dotnet", "DeployToProduction.WeatherForecast.App.dll"]