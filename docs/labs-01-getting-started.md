# Multitenant ASP.NET Core Lab 1: Getting started

This lab is about getting started with multitenant ASP.NET Core solution. Web application and environment 
where it is running need configuring. Configuration must be tested to make sure applications builds and runs. 
Last step is trying out something responsive - how application automatically reflects changes in tenants 
configuration file.

## Prerequisites

* Visual Studio 2019 or Visual Studio Code
* MSSQL (localdb is okay)
* MySQL (optional)

## Step 1: Configuring application

1. Clone solution to your machine
2. Open it and make sure it builds with no errors
3. Run your favorite text editor as administrator.
4. Open Windows hosts file (c:\windows\systems32\drivers\etc\hosts)
5. Add the following host mappings:
  * 127.0.0.1 sme1
  * 127.0.0.1 sme2
  * 127.0.0.1 bigcorp
6. Save file and close it

## Step 2: Running application
7. Run application NB! SAFE PROJECT (it doesn't open browser window)
8. Open browser and try out the following URL-s:
  * http://sme1:5000
  * http://sme2:5000
  * http://bigcorp:500
9. All these links should open front page of application and display tenant name
 
## Step 3: Changing tenant settings
 
1. Open tenants.json file (project root folder)
2. Change names of all tenants defined there
3. Save file 
4. Wait for 15 seconds until tenant definitions cache expires
4. Refresh all browser tabs that were opened in the end of previous step to see how tenant names changed on pages