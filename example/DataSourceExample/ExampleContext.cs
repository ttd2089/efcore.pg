namespace DataSourceExample;

internal class ExampleContext : DbContext
{
    public ExampleContext(DbContextOptions options) : base(options) {}
}
