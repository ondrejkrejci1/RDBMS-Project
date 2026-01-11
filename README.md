# Athletics Manager

This project is a desktop application created in C# (WPF) designed for the comprehensive management of athletic data. The application allows users to register athletes, sports clubs, competitions, and their results. It includes a system for importing data, automatically calculating statistics, and displaying top performances.

The application communicates with a Microsoft SQL Server database.

## 1. Quick Start (Pre-compiled)

To run the application immediately without installing development tools, follow these steps:

1. Download this repository as a **.zip** file (click the green **Code** button and select **Download ZIP**).
2. Extract (unzip) the downloaded file to any folder on your computer.
3. Open the extracted folder and navigate through the following path:
   `AthleticsManager` -> `bin` -> `Debug` -> `net8.0-windows`
4. In this folder, find and run the file **AthleticsManager.exe**.

### Configuration
For the application to work, it must connect to your MS SQL database.
1. In the same folder as the `.exe` file, find **AthleticsManager.dll.config**.
2. Open it in a text editor (e.g., Notepad).
3. Edit the `<appSettings>` section with your server address, database name, login, and password.
4. Save the file and run the application.

---

## 2. Advanced: Building from Source (CMD)

If you want to use your own database structure, modify the source code, or simply prefer to build the application yourself, follow this process.

### Prerequisites
* **.NET 8.0 SDK** (must be installed on your machine).
* **Microsoft SQL Server** (running instance).

### Step 1: Database Preparation
Before building the app, ensure your database is ready using the provided scripts.

1. Create a new empty database on your SQL Server.
2. **Create Structure:** Run the **`databaseScript.sql`** file found in the repository. This script creates all necessary tables (Athlete, Club, Result, etc.), views, and stored procedures.
3. **Import Static Data:** Run the **`importScript.sql`** file. This is crucial as it populates the `Discipline` and `Region` tables with required fixed IDs (e.g., 50m Run, High Jump, Regions). Without this step, the application will not function correctly.

### Step 2: Configuration and Building
1. Open your terminal (Command Prompt or PowerShell).
2. Navigate to the project folder where `AthleticsManager.csproj` is located.
   Example: `cd C:\Downloads\AthleticsManager\AthleticsManager`
3. **Configure Connection:** Before building, open the **`App.config`** file located in this folder using a text editor.
   * Update the values for `DataSource` (Server Address), `Database` (Name), `Name` (Login), and `Password` to match your local SQL Server instance.
   * Save the file.
4. Run the following command to compile and publish the application:
   
   `dotnet publish -c Release -r win-x64 --self-contained false`

### Step 3: Running the Application
1. Once the build finishes, navigate to the output folder:
   `bin\Release\net8.0-windows\win-x64\publish`
2. Since you configured the `App.config` in the previous step, the application is ready to use.
3. Find and run **AthleticsManager.exe**.
