# Banking Application

This is a C# Console App made to do tasks of a bank like Deposit, Withdraw and transfer funds.
This is built on **.NET** framework of version `.NET.5.0`

## Structure

This Solution Contains three projects

- ATM.CLI _`Depends on ATM.Services`_
- ATM.Models
- ATM.Services _`Depends on ATM.Models`_

## Banks Structure

![ClassDiagram](/ATM-Class-Diagram.png?raw=true 'ClassDiagram')

> Employees with Admin access can do more actions than normal staff.

## Data Storage

Data is stored in a local `bankdata.dat` file which is a binary file to store data.
Data fetching and data writing happens inside `DataHandler.cs` in `ATM.Services`.
Serializable C# objects are stored inside `bankdata.dat` using file stream and also can be deserialized when the data needs to be read.

## Flowchart

The flowchart including procedure of transactions

![Flowchart](/ATM_Console_App.jpg?raw=true 'Flowchart')
