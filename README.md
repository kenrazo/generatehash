# generatehash
Generate 40,000 hash

To create the database you must do the ff steps:
- Go to Infrastructure/DependencyInjection.cs
- Change the connection string to your local connection string
- Go to package manager console and use JobExam as Default Project
- Run Update-Database
- The database should be created.

To run the BackgroundWorker and the Api you must do the ff steps:
- Right click solutions
- Select Properties
- Go to Multiple Startup Projects
- Select Start in Action for BackgroundWorker and JobExam Project
- You can run the App now.

