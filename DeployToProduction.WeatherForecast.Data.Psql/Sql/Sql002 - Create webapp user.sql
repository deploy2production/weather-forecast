begin;

create user webapp with encrypted password 'webapppwd';

grant select, update, insert on table weather to webapp;

end;