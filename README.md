# DeployToProduction.WeatherForecast

This project is an example for blog https://deploy2production.ru/

## Development

Create network (https://www.tutorialworks.com/container-networking/):

> docker network create weather-forecast-net

info: 

Start Posgres docker container:

> docker run --name weather-forecast-postgres --net weather-forecast-net -p 5432:5432 -e POSTGRES_PASSWORD=postgres -d postgres

Start Redis docker conatainer:

> docker run --name weather-forecast-redis --net weather-forecast-net -p 6379:6379 -d redis

Build app container

> docker build -t weather-forecast-app-img -f Dockerfile .

Run app container

> docker run --name weather-forecast-app --net weather-forecast-net -p 5022:80 -d weather-forecast-app-img