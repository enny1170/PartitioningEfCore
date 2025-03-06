# PartitioningEfCore

This project demonstrates the implementation of database partitioning with Entity Framework Core. 
Because Table-Partitioning is not supported by EfCore for SQL-Server migrations. 
Database partitioning is a technique to divide large databases into smaller, more manageable parts, improving performance and maintainability.


## Features

- **EfCoreMigrationExtensions**: Implementation of addtional and changed Migration Operation for SQL-Server.
- **database**: Shows the use of the Attribute on Table Base
- **migrations**: Shows the use oft the new Operations. See also the PartitiontestContext, how to use the extensions in your context.

## Prerequisites

- .NET 6.0 or higher
- Entity Framework Core 6.0 or higher
- A supported database (e.g., SQL Server)

## Installation

1. Clone the repository:
    ```bash
    git clone https://github.com/username/PartitioningEfCore.git
    ```
2. Navigate to the project directory:
    ```bash
    cd PartitioningEfCore
    ```
3. Install the dependencies:
    ```bash
    dotnet restore
    ```

## Usage

1. Configure the database connection in `appsettings.json`.
2. Build a new migration
    ```bash
    dotnet ef migrations add testmigration
    ```
3. Show and check the generated sql-code
    ```bash
    dotnet ef migrations script
    ```
4. Apply the migrations:
    ```bash
    dotnet ef database update
    ```
5. Run the application:
    ```bash
    dotnet run
    ```

## License

This project is licensed under the MIT License. For more information, see the [LICENSE](LICENSE) file.

## Contribution

Contributions are welcome! Please read the [CONTRIBUTING](CONTRIBUTING.md) guidelines before submitting changes.
