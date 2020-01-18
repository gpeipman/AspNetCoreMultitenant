# Multi-tenant ASP.NET Core

This is demo project for my [Multi-tenant ASP.NET Core presentation](https://gunnarpeipman.com/presentations/aspnet-core-multitenant/). It demonstrates multi-tenant solutions I have worked out for ASP.NET Core. I'm using the same demo also for presentations. Solutions provided here are not absolute truth. There are cases when something different is needed and then the code here serves as a good starting point.

[Labs](docs/labs.md)

## Features

There are multiple features and solutions demonstrated in demo application:

* [Using database per tenant strategy](https://gunnarpeipman.com/aspnet-core-tenant-providers/)
* [Using shared database for tenants](https://gunnarpeipman.com/ef-core-global-query-filters/)
* [Multi-tenant EF Core database context with security net](https://gunnarpeipman.com/aspnet-core-defensive-database-context/)
* Loading tenants configuration
* [Missing tenant middleware](https://gunnarpeipman.com/aspnet-core-missing-tenant-middleware/)
* [Tenants provider to get current tenant](https://gunnarpeipman.com/aspnet-core-tenant-providers/)
* [Tenant-based dependency injection](https://gunnarpeipman.com/aspnet-core-tenant-based-dependency-injection/)
* Unit-testing multi-tenant database content (coming soon)
* [Multi-tenant configurable composite commands](https://gunnarpeipman.com/aspnet-core-configurable-composite-command/)

## Setting every thing up

To demonstrate multi-tenant features I have built I'm using SQL Server LocalDB and MySQL databases. SQL Server LocalDB uses integrated security. For MySQL some user account is needed to access database. Those who plan to run this example on Linux must use some Linux version of SQL Server or SQL Server running on some other machine.

There are three tenants defined:

* **bigcorp** - uses dedicated MySQL database
* **sme1** - uses shared SQL Server database
* **sme2** - uses shared SQL Server database

There are also some host name mappings needed to run this demo on local machine:
* 127.0.0.1    bigcorp
* 127.0.0.1    sme1
* 127.0.0.1    sme2

Web application uses port 5000 and it must be added to URL of all tenants:

* http://bigcorp:5000
* http://sme1:5000
* http://sme2:5000

When using public server to host this application make sure you turn on HTTPS in application startup.
