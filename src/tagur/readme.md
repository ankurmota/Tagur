# Microsoft Connect //2016 #
## Web and Data Application Development with Visual Studio 2017 and Azure ##

The following solution was created for Microsoft Connect 2016, Day 3, as a six-session series entitled **Web and Data Application Development with Visual Studio 2017 and Azure** and contains all code used in these sessions to create the cool image analysis tool [tagur](https://tagur.it "tagur") which is no longer fully actively, but can be created, extended, and refined for production use, as needed, **in your own environment**. 

NOTE: The tagur solution leverages a large number of Azure, Bot, Graph, and Cognitive Services required for full analysis and processing of images, and is for demonstration purposes only, and should not be considered ready for use in production environments.

The following servers and services are part of this solution. Some parts of the tagur solution are optional, or can be swapped out based on developer use and interest. (For example, this solution can process images stored in Azure SQL, SQL Server 2016, SQL Server for Linux, or Azure Blob Storage, or any combination of these.

1. Visual Studio 2017 
1. .NET Core 
1. ASP.NET Core 
1. Application Insights 
1. Power BI 
1. Azure Function Apps 
1. Azure Web Apps 
1. Azure SQL 
1. Azure DocumentDB 
1. Azure Object (Blob) Storage 
1. SQL Server 2016 
1. SQL Server for Linux 
1. SQL Server Data Tools 
1. Microsoft Office Graph 
1. Microsoft Cognitive Services 
1. Azure Search 
1. Azure Container Registries 
1. Azure Container Services 
1. Docker 
1. Azure Linux App Services 

Steps to modify and use tagur in your environment:

1. Go to *Common.cs* and replace any values within brackets "[]" with the values from your services, such as [YOUR GRAPH CLIENT ID] with the value created from the [Microsoft Application Registration Portal](https://apps.dev.microsoft.com/ "Microsoft Application Registration Portal").
1. Go to *appsettings.json* and replace any values with brackets "[]" with the values from your SQL Server server instances, such as [YOUR SQL SERVER FOR LINUX SERVER ADDRESS].
2. Go to Helpers > *SecretHelper.cs *and replace any values within brackets "[]" with the values for your SQL Server database instances and credentials, such as [YOUR SQL SERVER DATABASE NAME].

