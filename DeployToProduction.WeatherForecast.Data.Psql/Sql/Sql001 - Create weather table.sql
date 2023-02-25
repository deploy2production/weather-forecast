begin;

create table weather (
	date integer NOT NULL,
	location TEXT NOT NULL,
	temperature integer NOT NULL,
	kind integer NOT NULL
);

create unique index date_location_weather_idx on weather (date, location);

end;