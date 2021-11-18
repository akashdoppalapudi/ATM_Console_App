# Banking Application

This is a C# Console App made to do tasks of a bank like Deposit, Withdraw and transfer funds.
This is built on **.NET** framework of version `.NET.5.0`

## Structure

This Solution Contains three projects

- ATM.CLI _`Depends on ATM.Services`_
- ATM.Models
- ATM.Services _`Depends on ATM.Models`_

## Data Storage

Data is stored in an **MYSQL Database** data is added, modified and retrieved using MySqlClient which is a `Nuget package`.

The Database Schema is given in `schema.sql`.

The schema is available at [`https://drawsql.app/akash/diagrams/banking-application`](https://drawsql.app/akash/diagrams/banking-application)

The database can be created using the following command

```PowerShell
> mysql -u root -p "name of the database" < "path to the schema sql"
```


## Flowchart

The flowchart including procedure of transactions

![Flowchart](/ATM_Console_App.jpg?raw=true 'Flowchart')
