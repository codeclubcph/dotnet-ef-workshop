# Scripting migrations
> 5 minutes

Applying migrations from classes directly to a database is considered a poor practice.

A commonly applied anti-pattern is to run pending migrations as part of an application's startup process. **Don't do this**.
This is not a recommended approach and may cause tremendous headaches down the road.

The recommended approach is to generate an idempotent SQL script based on all your migrations, inspect them carefully, 
and run the SQL script against the target database.

Generating an SQL script allows you to hand it off to a database administrator for approval before 
messing with their database.

## Task 1: Generate an SQL script

We'll use the `dotnet-ef` tool to generate the idempotent SQL script.

> From the `tests/Demo` folder run `dotnet ef migrations script -i -o Scripts/sql.db`

Now inspect the script placed in the folder `tests/Demo/Scripts`.

### Additional reading
> [Read more on applying migrations here](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli).


### [Go to next step](../04Relations/readme.md)