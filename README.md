Minimal repro to investigate DNX Windows Service with EF supposedly being able
to migrate a database on `beta8` but failing on `rc1-final`.

Using these commands to publish the service for each runtime:

```
dnu publish --runtime dnx-clr-win-x86.1.0.0-beta8 --configuration Release --no-source -o C:\Temp\out
dnu publish --runtime dnx-clr-win-x86.1.0.0-rc1-final --configuration Release --no-source -o C:\Temp\out
```

Install as a Windows Service:


```
sc create EfPermissions binPath= "C:\Temp\out\approot\release.cmd"
```

Without `dbcreator` role, both runtimes fail:

![image](https://cloud.githubusercontent.com/assets/2253814/11681766/d5d0b346-9ec5-11e5-8f19-865eba63ff66.png)


```
2015-12-09 22:33:37.882 +13:00 [Information] Starting...
2015-12-09 22:33:37.944 +13:00 [Information] Setting up database.
2015-12-09 22:33:38.069 +13:00 [Information]    Deleted.
2015-12-09 22:33:38.101 +13:00 [Fatal]   Oh noes, can't migrate.
System.Data.SqlClient.SqlException (0x80131904): CREATE DATABASE permission denied in database 'master'.
   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.SqlInternalConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlCommand.RunExecuteNonQueryTds(String methodName, Boolean async, Int32 timeout, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.InternalExecuteNonQuery(TaskCompletionSource`1 completion, String methodName, Boolean sendToPipe, Int32 timeout, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.ExecuteNonQuery()
   at Microsoft.Data.Entity.Storage.Internal.RelationalCommand.<>c.<ExecuteNonQuery>b__13_0(DbCommand cmd, IRelationalConnection con)
   at Microsoft.Data.Entity.Storage.Internal.RelationalCommand.Execute[T](IRelationalConnection connection, Func`3 action, String executeMethod, Boolean openConnection, Boolean closeConnection)
   at Microsoft.Data.Entity.Storage.Internal.RelationalCommand.ExecuteNonQuery(IRelationalConnection connection, Boolean manageConnection)
   at Microsoft.Data.Entity.Storage.RelationalCommandExtensions.ExecuteNonQuery(IEnumerable`1 commands, IRelationalConnection connection)
   at Microsoft.Data.Entity.Storage.Internal.SqlServerDatabaseCreator.Create()
   at Microsoft.Data.Entity.Migrations.Internal.Migrator.Migrate(String targetMigration)
   at Microsoft.Data.Entity.RelationalDatabaseFacadeExtensions.Migrate(DatabaseFacade databaseFacade)
   at EfPermissions.Service.Program.OnStart(String[] args)
ClientConnectionId:e7372692-6d18-4e5a-bfd6-ceacdab3094c
Error Number:262,State:1,Class:14
```

With `dbcreator` role, both runtimes succeed:

![image](https://cloud.githubusercontent.com/assets/2253814/11681824/1c0029c8-9ec6-11e5-82d8-338ea06598d7.png)

```
2015-12-09 22:34:55.749 +13:00 [Information] Starting...
2015-12-09 22:34:55.811 +13:00 [Information] Setting up database.
2015-12-09 22:34:55.936 +13:00 [Information]    Deleted.
2015-12-09 22:34:56.177 +13:00 [Information]    Migrated.
2015-12-09 22:34:56.177 +13:00 [Information] Started.
```
