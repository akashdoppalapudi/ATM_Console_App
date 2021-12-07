# Banking Application

This is a C# Console App made to do tasks of a bank like Deposit, Withdraw and transfer funds.
This is built on **.NET** framework of version `.NET.6.0`

## Structure

This Solution Contains three projects

- ATM.CLI _`Depends on ATM.Services`_
- ATM.Models
- ATM.Services _`Depends on ATM.Models`_

## Dependencies

>Microsoft.EntityFrameworkCore.SqlServer

>Microsoft.EntityFrameworkCore.Tools

>Microsoft.EntityFrameworkCore.Design

>AutoMapper

## Data Storage

Data is stored in an **Microsoft SQL Server** data is added, modified and retrieved using `Microsoft.EntityFrameWorkCore.SqlServer` which is a `Nuget package`.

Changes to the DB Model are updated through migration using...

```PowerShell
add-migration <migration-name>
```
```PowerShell
update-database <target-migration>
```

**Code First** approach is used to create the database in Microsoft SQL Server.


## Flowchart

The flowchart including procedure of transactions

![Flowchart](/ATM_Console_App.jpg?raw=true 'Flowchart')
