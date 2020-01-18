# Multitenant ASP.NET Core Lab 1: Getting started

This lab is about getting started with multitenant ASP.NET Core solution. 

## Prerequisites

* Visual Studio 2019 or Visual Studio Code
* MSSQL (localdb is okay)
* MySQL (optional)
* Azure storage emulator

## Exercise

1. Clone solution to your machine
2. Open it and make sure it builds with no errors
3. Run your favorite text editor as administrator.
4. Open Windows hosts file (c:\windows\systems32\drivers\etc\host)
5. Add the following host mappings:
  * 127.0.0.1 sme1
  * 127.0.0.1 sme2
  * 127.0.0.1 bigcorp
6. Save file and close it