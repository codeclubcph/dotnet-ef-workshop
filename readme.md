![ccc-logo](docs/imgs/ccc-github-workshop-banner.png)

# TBSI .NET and Databases workshop

Fast-paced and in-depth workshop on broadly applicable principles, practices, and standards for working with databases 
through code using C# Entity Framework Core.

This workshop is created by [Nicklas Millard](https://www.linkedin.com/in/nicklasmillard/) as part of a [Code Club Copenhagen - 3C meetup
workshop](https://www.meetup.com/code-club-copenhagen/events/299885651/).

In this workshop, I have gathered years of lessons learned from real-world projects, both small and large. If you have no or little
experience with .NET and EntityFramework Core (EF), then I suggest you [read this quick EF recap first](docs/ef-recap.md).

## Version check

Make sure you have the right software installed. You will need:
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- .NET capable IDE such as JetBrains Rider or Visual Studio
- Docker

## What you'll learn

In this workshop, you'll learn how to effectively and efficiently work with .NET and databases through EntityFramework
Core (EF). We'll go through various topics such as
- advanced entity configuration using `IEntityConfiguration<>`,
- setting up a production ready `DbContext`,
- how to get fast feedback by using testcontainers with a real postgres server when testing,
- configuring entity relations,
- table per hierarchy inheritance and 
- polymorphism using JSON

## Workshop steps

- [01 – Environment check](tests/Demo/WorkshopSteps/01Setup/readme.md)
- [02 – Rapid feedback using testcontainers](tests/Demo/WorkshopSteps/02RapidFeedback/readme.md)
- [03 – Entity Configuration](tests/Demo/WorkshopSteps/03EntityConfiguration/readme.md)
- [04 – Relations](tests/Demo/WorkshopSteps/04Relations/readme.md)
- [05 – Inheritance](tests/Demo/WorkshopSteps/05Inheritance/readme.md)
- [06 – Json](tests/Demo/WorkshopSteps/06Json/readme.md)