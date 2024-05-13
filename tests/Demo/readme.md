# Workshop project overview

## Folders

### `Shared`

This folder holds a few different classes that are used throughout the workshop and are meant to be adjusted as you
follow along each workshop step.

- `AppDbContext`: the data access class.
- `AppDbContextDesignTimeFactory`: a class that allows you to run `dotnet-ef` commands without a startup project.
- `Models`: file that holds our different domain models.
- `ModelEntityConfigurations`: database configuration for the domain models.
- `DatabaseFixture`: fixture that starts a postgres container used for testing (implemented during step 2).
- `DbContextFixture`: helper class that creates an `AppDbContext` used for testing

### `WorkshopSteps`

Folders containing the different steps that you follow during the workshop.

### `Pitfalls`

Few examples of why using an in-memory database is not sufficient for testing database interactions.