# Multitenant ASP.NET Core Lab 2: Tenant sources

**NB** TenantProvider => TenantSource

Tenants served by application have different settings and these settings must be held somewhere. 
It can relational database, configuration file on server disk or file on some cloud storage account. 
Tenants store - the place where tenant definitions are held - can be also web API end-point.

This lab is about trying out different tenant sources. Tenant sources are classes to communicate 
with tenants store.

## Prerequisites

* Visual Studio 2019 or Visual Studio Code
* MSSQL (localdb is okay)
* One of these two:
	* Azure account
	* Azure storage emulator on local machine
* Azure Storage Explorer

## Step 1: File tenant source

NB! Make file tenant source configurable

## Step 2: Database tenant source

1. Add new connection string to appSettings.Debug.json file 
2. Make it point to existing or new database
3. Run database script (ADD IT TO SOLUTION FOLDER) against new database
4. Open Startup class of web application and find row where ITenantSource is matched with FileTenantSource
5. Replace FileTenantSource with DatabaseTenantSource
6. Connect to database you created before, open tenants table and add DB to end of every tenant name
7. Build web application and run it
8. Open browser and try out the following URL-s:
  * http://sme1:5000
  * http://sme2:5000
  * http://bigcorp:500
9. All pages should have DB after tenant name in their title

## Step 3: Blob storage tenant provider

**NB!** Refer to materials for Azure Storage Explorer and Azure storage emulator