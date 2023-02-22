# Summary

When a NpgsqlDataSource is available via DI it it only used by the first instance of DbContext
that's resolved. Subsequent instances will construct but are unable to make connections.

## Reproduction Steps

Bring up the compose environment in the ./example directory:

```
docker compose up -d
```

Curl the `/test` endpoint twice:

```
❯ curl -i localhost:8080/test
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Wed, 22 Feb 2023 04:57:59 GMT
Server: Kestrel
Transfer-Encoding: chunked

passed

❯ curl -i localhost:8080/test
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Wed, 22 Feb 2023 04:58:00 GMT
Server: Kestrel
Transfer-Encoding: chunked

failed: InvalidOperationException: The ConnectionString property has not been initialized.
   at Npgsql.NpgsqlConnection.Open(Boolean async, CancellationToken cancellationToken)
   at Npgsql.NpgsqlConnection.OpenAsync(CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenDbConnectionAsync(Boolean errorsExpected, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenInternalAsync(Boolean errorsExpected, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenInternalAsync(Boolean errorsExpected, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
   at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteNonQueryAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions.ExecuteSqlRawAsync(DatabaseFacade databaseFacade, String sql, IEnumerable`1 parameters, CancellationToken cancellationToken)
   at Program.<>c.<<<Main>$>b__0_3>d.MoveNext() in /src/example/DataSourceExample/Program.cs:line 29
```
