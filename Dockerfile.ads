FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Copy everything
COPY . ./


# Build and publish a release
RUN dotnet publish DeployToProduction.Ads.WebApi --configuration Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
EXPOSE 80

COPY --from=build /out .

# Start App
ENTRYPOINT ["dotnet", "DeployToProduction.Ads.WebApi.dll"]