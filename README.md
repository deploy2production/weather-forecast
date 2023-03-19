# DeployToProduction.WeatherForecast

This project is an example for blog https://deploy2production.ru/

## Development

Start app:

> docker compose -f docker-compose.db.yml -f docker-compose.app.dev.yml up

> docker compose -f docker-compose.db.yml -f docker-compose.app.prd.yml up

Start setup tool:

> docker compose -f docker-compose.db.yml -f docker-compose.setup.yml up

Shotdown app:

> docker compose -f docker-compose.db.yml -f docker-compose.app.dev.yml down

> docker compose -f docker-compose.db.yml -f docker-compose.app.prd.yml down