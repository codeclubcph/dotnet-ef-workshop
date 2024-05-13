# Environment setup
> 5 minutes

## Test check
Ensure that your machine is correctly set up by running the `RunThis` test in [EnvironmentCheckTest.cs](EnvironmentCheckTest.cs).

## Docker check
Try to run the [`docker-compose.yml`](../../../../docker-compose.yml) file at the solution root.

```yaml
version: "3.9"

services:
  db:
    image: postgres:16.2
    ports:
      - "5499:5432"
    environment:
      - POSTGRES_USER=postgres
      - POSTGRES_PASSWORD=postgres
```

> Run the docker CLI command `docker-compose up`.

Notice that the mapped port on your machine is `5499` and not the postgres default `5432`. This was simply done to avoid collision with
your existing ports in case you already have a running postgres server.
