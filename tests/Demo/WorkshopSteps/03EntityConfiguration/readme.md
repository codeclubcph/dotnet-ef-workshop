# 03 – Entity configuration

Effective and efficient programmatic database management is hugely important to work confidently
with a code-first approach.

In this step, we'll configure a `DbContext`. The `DbContext` is the access point to your database.
Then, we'll configure the database tables based on our entities using `IEntityTypeConfiguration<>`.

To generate migrations based on our configured entities, we need to use the dotnet-ef CLI tool.
In this solution, we don't have a "startup project" (runnable application), meaning that we'll need to create a
design time `DbContext` factory.

### [Go to first task](01-initial-configuration.md)