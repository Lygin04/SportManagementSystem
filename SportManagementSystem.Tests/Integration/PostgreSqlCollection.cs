namespace SportManagementSystem.Tests.Integration;

[CollectionDefinition(Name)]
public sealed class PostgreSqlCollection : ICollectionFixture<PostgreSqlTestContainerFixture>
{
    public const string Name = "PostgreSql";
}
