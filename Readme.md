# Banking Application

This is a C# Console App made to do tasks of a bank like Deposit, Withdraw and transfer funds.
This is built on **.NET** framework of version `.NET.6.0`

## Structure

This Solution Contains three projects

- ATM.CLI _`Depends on ATM.Services`_
- ATM.Models
- ATM.Services _`Depends on ATM.Models`_

## Dependencies

>MySql.EntityFrameWorkCore

## Data Storage

Data is stored in an **MYSQL Database** data is added, modified and retrieved using `MySql.EntityFrameWorkCore` which is a `Nuget package`.

**Code First** approach is used to create the database in MySql Server.


## Flowchart

The flowchart including procedure of transactions

![Flowchart](/ATM_Console_App.jpg?raw=true 'Flowchart')
