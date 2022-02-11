start_postgres
`psql --username=postgres --host=localhost`

```psql
\c template1
create table access_log(timestamp TIMESTAMP, latitude float, longitude float, visitorid char(37));
\q
```