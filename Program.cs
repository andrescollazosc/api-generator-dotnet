using System.Diagnostics;
using Spectre.Console;

public class Program
{
    public static void Main()
    {
        var projectName = AnsiConsole.Ask<string>("[green]Enter solution name:[/]");
        var serviceName = AnsiConsole.Ask<string>("[green]Please, enter the service name:[/]");
        
        projectName = projectName.Replace(" ", "");
        serviceName = serviceName.Replace(" ", "");

        var structureProjectName = string.IsNullOrEmpty(serviceName) ? $"{projectName}" : $"{projectName}.{serviceName}";
        var apiProjectName = $"{structureProjectName}.Api";
        var coreProjectName = $"{structureProjectName}.Core";
        var dataProjectName = $"{structureProjectName}.Data";
        
        var basePath = AnsiConsole.Ask<string>("[green]Please, enter a path:[/]");
        var solutionPath = Path.Combine(basePath, projectName);

        Directory.CreateDirectory(solutionPath);
        Directory.SetCurrentDirectory(solutionPath);

        Console.WriteLine();
        AnsiConsole.MarkupLine($"[blue]📁  Creating project:[/] [yellow]{projectName}[/]");
        // Create solution
        RunCommand($"dotnet new sln -n {projectName}", solutionPath);

        // Create projects
        RunCommand($"dotnet new webapi -n {apiProjectName}", solutionPath);
        RunCommand($"dotnet new classlib -n {coreProjectName}", solutionPath);
        RunCommand($"dotnet new classlib -n {dataProjectName}", solutionPath);

        // Add projects to solution
        RunCommand($"dotnet sln add {apiProjectName}/{apiProjectName}.csproj", solutionPath);
        RunCommand($"dotnet sln add {coreProjectName}/{coreProjectName}.csproj", solutionPath);
        RunCommand($"dotnet sln add {dataProjectName}/{dataProjectName}.csproj", solutionPath);

        // Add Core reference to API
        RunCommand($"dotnet add {apiProjectName}/{apiProjectName}.csproj reference {coreProjectName}/{coreProjectName}.csproj", solutionPath);
        RunCommand($"dotnet add {apiProjectName}/{apiProjectName}.csproj reference {dataProjectName}/{dataProjectName}.csproj", solutionPath);

        // Add Core reference to Data
        RunCommand($"dotnet add {dataProjectName}/{dataProjectName}.csproj reference {coreProjectName}/{coreProjectName}.csproj", solutionPath);
        
        AnsiConsole.MarkupLine("[green]✅  Solution created successfully![/]");
        
        AnsiConsole.MarkupLine($"[blue]🛠️\n  Install packages[/]");
        AddNuGetPackage(apiProjectName, "AutoMapper.Extensions.Microsoft.DependencyInjection", "12.0.1", solutionPath);
        AddNuGetPackage(coreProjectName, "FluentValidation.AspNetCore", "", solutionPath);
        AddNuGetPackage(dataProjectName, "Microsoft.EntityFrameworkCore", "7.0.20", solutionPath);
        AddNuGetPackage(dataProjectName, "Npgsql.EntityFrameworkCore.PostgreSQL", "7.0.18", solutionPath);
        AnsiConsole.MarkupLine("[green]✅  Solution created successfully![/]");

        CreateCoreProjectStructure(solutionPath, coreProjectName, serviceName);;
    }

    private static void CreateCoreProjectStructure(string workingDirectory, string projectName, string serviceName)
    {
        AnsiConsole.MarkupLine($"[blue]🛠️\n  Creating Core project structure[/]");
        var coreProjectPath = Path.Combine(workingDirectory, projectName);
        
        var class1Path = Path.Combine(coreProjectPath, "Class1.cs");
        if (File.Exists(class1Path))
        {
            File.Delete(class1Path);
            AnsiConsole.MarkupLine("[yellow]✔ Class1.cs was deleted[/]");
        }

        CreateFolder(coreProjectPath, serviceName);
        CreateFolder(Path.Combine(coreProjectPath, serviceName), "Models");
        CreateFolder(Path.Combine(coreProjectPath, serviceName), "Services");
        CreateFolder(Path.Combine(coreProjectPath, serviceName), "Repositories");
        CreateFolder(Path.Combine(coreProjectPath, serviceName), "Validators");
    }

    private static void CreateFolder(string rootFolder, string folderName)
    {
        var newFolder = Path.Combine(rootFolder, folderName);
        Directory.CreateDirectory(newFolder);
        AnsiConsole.MarkupLine($"[green]📁 Folder created:[/] [yellow]{folderName}[/]");
    }

    private static void AddNuGetPackage(string project, string packageName, string version, string workingDirectory)
    {
        Console.WriteLine($"Installing {packageName}...");
        
        var versionArgument = string.IsNullOrEmpty(version) ? "" : $"--version {version}";
        
        RunCommand($"dotnet add {project}/{project}.csproj package {packageName} {versionArgument}", workingDirectory);
    }

    private static void RunCommand(string command, string workingDirectory)
    {
        string shell, shellArgs;

        if (OperatingSystem.IsWindows())
        {
            shell = "cmd.exe";
            shellArgs = $"/c {command}";
        }
        else
        {
            shell = "/bin/bash";
            shellArgs = $"-c \"{command}\"";
        }
        
        var startInfo = new ProcessStartInfo
        {
            FileName = shell,
            Arguments = shellArgs,
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        
        AnsiConsole.MarkupLine($"[blue]→ Running:[/] [yellow]{command}[/]");

        using var process = new Process();
        process.StartInfo = startInfo;
        process.Start();

        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        if (!string.IsNullOrEmpty(output))
            AnsiConsole.MarkupLine($"[green]{EscapeMarkup(output)}[/]");
        if (!string.IsNullOrEmpty(error))
            AnsiConsole.MarkupLine($"[red]{EscapeMarkup(error)}[/]");
    }
    
    private static string EscapeMarkup(string input)
    {
        return input.Replace("[", "[[").Replace("]", "]]");
    }
    
}




