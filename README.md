# DeployToProduction.WeatherForecast

This project is an example for blog https://deploy2production.ru/

## Development

Start Posgres docker container:

> docker run --name weather-forecast-postgres -p 5432:5432 -e POSTGRES_PASSWORD=postgres -d postgres

Start Redis docker conatainer:

> docker run --name weather-forecast-redis -p 6379:6379 -d redis