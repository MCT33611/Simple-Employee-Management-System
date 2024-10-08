# Simple Employee Management System

A comprehensive employee management solution built with .NET 8 MVC, designed to streamline workforce management tasks with a responsive user interface.

## Table of Contents
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Usage](#usage)
- [Contributing](#contributing)

## Features

- Employee CRUD operations
- Department management
- Project tracking
- Dependent information management
- JSON data import functionality
- Responsive user interface for seamless access across devices
- Advanced employee filtering options:
  - Filter by department
  - Filter by date of joining
  - Filter by salary range
- Responsive design for optimal viewing on desktop, tablet, and mobile devices

## Prerequisites

Before you begin, ensure you have met the following requirements:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) installed on your machine
- A compatible IDE (e.g., Visual Studio 2022, Visual Studio Code)
- Git for version control

## Installation

Follow these steps to get your development environment set up:

1. Install .NET 8 SDK:
   - Visit the [official .NET download page](https://dotnet.microsoft.com/download/dotnet/8.0)
   - Download and install the appropriate version for your operating system

2. Clone the repository:
   ```
   git clone https://github.com/MCT33611/Simple-Employee-Management-System.git
   ```

3. Navigate to the project directory:
   ```
   cd Simple-Employee-Management-System
   ```

4. Restore the project dependencies:
   ```
   dotnet restore
   ```

5. Update the database connection string in `appsettings.json` to point to your local SQL Server instance.

6. Apply database migrations:
   ```
   dotnet ef database update
   ```

## Usage

To run the application:

1. Navigate to the project directory in your terminal.

2. Run the following command:
   ```
   dotnet run
   ```

3. Open a web browser and go to `https://localhost:5001` or `http://localhost:5000`.

4. Use the navigation menu to access different features:
   - Employee List: View and manage all employees
     - Use the filtering options to narrow down the employee list by department, date of joining, or salary range
   - Department Management: Add, edit, or delete departments
   - Employee Creation: Add new employees with their details
   - JSON Import: Bulk import employee data using JSON format

5. The responsive design ensures a seamless experience across desktop, tablet, and mobile devices. Try accessing the application on different devices to see how the layout adapts.

## Contributing

Contributions to the Simple Employee Management System are welcome. Please follow these steps to contribute:

1. Fork the repository
2. Create a new branch (`git checkout -b feature/AmazingFeature`)
3. Make your changes
4. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
5. Push to the branch (`git push origin feature/AmazingFeature`)
6. Open a Pull Request

