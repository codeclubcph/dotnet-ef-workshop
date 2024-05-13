namespace Tbsi.Workshop.EfCore.Demo.Shared;

public static class DbConstants
{
    /// <summary>
    /// Use this property to connect to your postgres database started by the 'docker-compose up'.
    /// </summary>
    public const string PostgresConnectionString = "Host=localhost; Port=5499; User ID=postgres; Password=postgres; Database=postgres";
}