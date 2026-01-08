# Athletics Manager

This project is a desktop application created in C# (WPF) designed for the comprehensive management of athletic data. The application allows users to register athletes, sports clubs, competitions, and their results. It includes a system for importing data, automatically calculating statistics, and displaying top performances.

The application communicates with a Microsoft SQL Server database.

## How to Run the Application

To run the application, you do not need to install Visual Studio. You can simply use the compiled executable file found in this repository. Please follow these steps:

1. Download this repository as a **.zip** file (click the green **Code** button and select **Download ZIP**).
2. Extract (unzip) the downloaded file to any folder on your computer.
3. Open the extracted folder and navigate through the following path:
   `AthleticsManager` -> `bin` -> `Debug` -> `net6.0-windows` (or a similar .NET version folder).
4. In this folder, find and run the file **AthleticsManager.exe**.

## Database Configuration

For the application to work correctly, it must be connected to your instance of an MS SQL database. The connection is not hardcoded; you can modify it in the configuration file.

In the same folder where the `.exe` file is located, find the file named **AthleticsManager.dll.config**.

Open this file in any text editor (e.g., Notepad) and look for the `<appSettings>` section. Update the following values according to your database settings:

* **DataSource** - The address of your server.
* **Database** - The name of your database.
* **Name** - The username for SQL login.
* **Password** - The password for the database.

After saving the changes in this file, simply restart the application, and it should automatically connect to your server.
