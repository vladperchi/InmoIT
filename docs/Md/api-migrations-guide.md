# Migration

## Documentation

InmoIt currently operates with a MSSQL database provider. In future versions Postgres and MySql could be included.

Firstly, you need to make sure that valid connection strings are mentioned in the appSetting.json
Next, set either to true in appSetting under `PersistenceSettings`.

`"UseMsSql": true,`

### Important

- Make sure to delete all the migrations, and re-add migrations via the below CLI Command.
- Make sure that you drop the existing database if any.

## Steps

- Navigate to each of the Infrastructure project per module and shared(Shared.Infrastructure)
- Open the directory in terminal mode. You just have to right the Infrastructure project in Visual Studio and select `Open in Terminal`.

![image](https://user-images.githubusercontent.com/31455818/122291148-1d211380-cf12-11eb-9f28-35e5ec0989e5.png)

- Run the EF commands. You can find the EF Commands below in the next section with additional steps ;)
- That's it!

### Application

Navigate terminal to Shared.Infrastructure and run the following.

`dotnet ef migrations add "initial" --startup-project ../../API -o Persistence/Migrations/ --context ApplicationDbContext`

### Identity

Navigate terminal to Modules.Identity.Infrastructure and run the following.

`dotnet ef migrations add "initial" --startup-project ../../../API -o Persistence/Migrations/ --context IdentityDbContext`

### Flow

Navigate terminal to Modules.Flow.Infrastructure and run the following.

`dotnet ef migrations add "initial" --startup-project ../../../API -o Persistence/Migrations/ --context FlowDbContext`

### Document

Navigate terminal to Modules.Document.Infrastructure and run the following.

`dotnet ef migrations add "initial" --startup-project ../../../API -o Persistence/Migrations/ --context DocumentDbContext`
