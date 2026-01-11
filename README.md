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

If you want to use your own database structure or build the application from scratch, follow this process.

### Prerequisites
* **.NET 8.0 SDK** (must be installed on your machine).
* **Microsoft SQL Server** (running instance).

### Step 1: Database Preparation
1. Create a new empty database on your SQL Server.
2. **Install Database:** Run the **`install_database.sql`** file found in the repository. This script creates the structure and imports static data in a single transaction.

### Step 2: Configuration & Build
1. Open your terminal (Command Prompt or PowerShell).
2. Navigate to the project folder where `AthleticsManager.csproj` is located.
   Example: `cd C:\Downloads\AthleticsManager\AthleticsManager`
3. **Edit Configuration:**
   * Find and open the file named **`App.config`** in this folder using a text editor.
   * Update the `<appSettings>` section (DataSource, Database, Name, Password) to match your local SQL Server.
   * Save the changes.
4. **Build the Application:**
   Run the following command to compile the project with your configuration:
   
   `dotnet publish -c Release -r win-x64 --self-contained false`

### Step 3: Running the Application
1. Navigate to the output folder created by the build process:
   `bin\Release\net8.0-windows\win-x64\publish`
2. Find and run **AthleticsManager.exe**.
3. The application will launch using the configuration you defined in `App.config` before the build.

## Help & Support

If you run into any issues with the installation or have questions about the implementation, feel free to reach out.

* **Email:** [krejci3@spsejecna.cz](mailto:krejci3@spsejecna.cz)
* **Issues:** If you find a bug, please open an issue in this repository.
