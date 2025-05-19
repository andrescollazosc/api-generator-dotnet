using ApiGenerator.DotNet.Common;
using ApiGenerator.DotNet.Templates;
using Spectre.Console;

public class Program
{
    public static void Main()
    {
        var projectName = AnsiConsole.Ask<string>("[green]Enter solution name:[/]");
        var serviceName = AnsiConsole.Ask<string>("[green]Please, enter the service name:[/]");
        var basePath = AnsiConsole.Ask<string>("[green]Please, enter a path:[/]");
        
        projectName = projectName.Replace(" ", "");
        serviceName = serviceName.Replace(" ", "");

        var structureProjectName = string.IsNullOrEmpty(serviceName) ? $"{projectName}" : $"{projectName}.{serviceName}";
        var apiProjectName = $"{structureProjectName}.Api";
        var coreProjectName = $"{structureProjectName}.Core";
        var dataProjectName = $"{structureProjectName}.Data";
        
        var solutionPath = Path.Combine(basePath, projectName);

        Directory.CreateDirectory(solutionPath);
        Directory.SetCurrentDirectory(solutionPath);

        Console.WriteLine();
        
        var template = new ArchitectureTemplate(projectName, solutionPath, apiProjectName, coreProjectName, dataProjectName);
        template.CreateScaffold();
        
        AnsiConsole.MarkupLine("[green]✅  Solution created successfully![/]");
        
        AnsiConsole.MarkupLine($"[blue]🛠️\n  Install packages[/]");
        AddNuGetPackage(apiProjectName, "AutoMapper.Extensions.Microsoft.DependencyInjection", "12.0.1", solutionPath);
        AddNuGetPackage(coreProjectName, "FluentValidation.AspNetCore", "", solutionPath);
        AddNuGetPackage(dataProjectName, "Microsoft.EntityFrameworkCore", "7.0.20", solutionPath);
        AddNuGetPackage(dataProjectName, "Npgsql.EntityFrameworkCore.PostgreSQL", "7.0.18", solutionPath);
        AnsiConsole.MarkupLine("[green]✅  Solution created successfully![/]");

        template.CreateCoreProjectStructure(solutionPath, coreProjectName, serviceName);;
    }

    private static void AddNuGetPackage(string project, string packageName, string version, string workingDirectory)
    {
        Console.WriteLine($"Installing {packageName}...");
        
        var versionArgument = string.IsNullOrEmpty(version) ? "" : $"--version {version}";
        
        Executables.RunCommand($"dotnet add {project}/{project}.csproj package {packageName} {versionArgument}", workingDirectory);
    }
    
}

