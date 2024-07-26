#! /bin/bash

echo 'Assuming you have docker installed on your machine. If not, please install docker first.'

echo 'Pulling the latest version of SQL Server 2019 on Ubuntu 16.04 from Microsoft Container Registry (MCR)'
docker pull mcr.microsoft.com/mssql/server

echo 'Running the SQL Server 2019 container'
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Password123456" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest

echo 'SQL Server 2019 container is running'

echo 'run the command "update-database" inside VisualStudio click Tools -> NuGet Package Manager -> Package Manager Console'
echo 'Select the "Default project" as "src\Application'

