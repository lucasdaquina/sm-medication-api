# sm-medication-api
SmartMed Back End code challenge


Steps to run api:

1. Clone the repository
2. Open the project in your favorite IDE (Visual Studio)    
3. Open terminal/cmd and navigate to the solution folder
3. Run Init.sh with the command .\Init.sh to pull docker sql server image and run the container
4. After the sql server's running
    - On VisualStudio click Tools -> NuGet Package Manager -> Package Manager Console
    - Select the "Default project" as "src\Application"
    - run the command dotnet ef database update --project .\src\SM.Medication.Infrastructure\SM.Medication.Infrastructure.csproj --startup-project .\src\SM.Medication.Api\SM.Medication.Api.csproj"
5. Select SM.Medication.API Project as startup and run https.

Now the Solution is ready, with a base SQL Server DB running with 3 medications on the medication table already.

PS: The token to call the API is "SmartMed eyAiVG9rZW4iOiAiMTIzIiwgIlJvbGUiOiAiQWRtaW4ifQ=="
