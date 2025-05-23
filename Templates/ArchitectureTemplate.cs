using ApiGenerator.DotNet.Common;
using ApiGenerator.DotNet.Common.Helpers;
using ApiGenerator.DotNet.Scaffolding;
using Humanizer;
using Spectre.Console;

namespace ApiGenerator.DotNet.Templates;

public class ArchitectureTemplate
{
    private readonly string _projectName;
    private readonly string _solutionPath;
    private readonly string _apiProjectName;
    private readonly string _coreProjectName;
    private readonly string _dataProjectName;

    public ArchitectureTemplate(string projectName,
        string solutionPath,
        string apiProjectName,
        string coreProjectName,
        string dataProjectName)
    {
        _projectName = projectName;
        _solutionPath = solutionPath;
        _apiProjectName = apiProjectName;
        _coreProjectName = coreProjectName;
        _dataProjectName = dataProjectName;
    }

    public void CreateScaffold()
    {
        Executables.RunCommand($"dotnet new sln -n {_projectName}", _solutionPath);
        
        // Create projects
        Executables.RunCommand($"dotnet new webapi -n {_apiProjectName}", _solutionPath);
        Executables.RunCommand($"dotnet new classlib -n {_coreProjectName}", _solutionPath);
        Executables.RunCommand($"dotnet new classlib -n {_dataProjectName}", _solutionPath);
        
        // Add projects to solution
        Executables.RunCommand($"dotnet sln add {_apiProjectName}/{_apiProjectName}.csproj", _solutionPath);
        Executables.RunCommand($"dotnet sln add {_coreProjectName}/{_coreProjectName}.csproj", _solutionPath);
        Executables.RunCommand($"dotnet sln add {_dataProjectName}/{_dataProjectName}.csproj", _solutionPath);
        
        // Add Core reference to API
        Executables.RunCommand($"dotnet add {_apiProjectName}/{_apiProjectName}.csproj reference {_coreProjectName}/{_coreProjectName}.csproj", _solutionPath);
        Executables.RunCommand($"dotnet add {_apiProjectName}/{_apiProjectName}.csproj reference {_dataProjectName}/{_dataProjectName}.csproj", _solutionPath);
        
        // Add Core reference to Data
        Executables.RunCommand($"dotnet add {_dataProjectName}/{_dataProjectName}.csproj reference {_coreProjectName}/{_coreProjectName}.csproj", _solutionPath);
    }
    
    public void CreateCoreProjectStructure(string workingDirectory, string projectName, string serviceName)
    {
        AnsiConsole.MarkupLine($"[blue]🛠️\n  Creating Core project structure[/]");
        var coreProjectPath = Path.Combine(workingDirectory, projectName);
        
        var class1Path = Path.Combine(coreProjectPath, "Class1.cs");
        if (File.Exists(class1Path))
        {
            File.Delete(class1Path);
            AnsiConsole.MarkupLine("[yellow]✔ Class1.cs was deleted[/]");
        }

        FileSystemHelper.CreateFolder(coreProjectPath, serviceName);
        FileSystemHelper.CreateFolder(Path.Combine(coreProjectPath, serviceName), "Models");
        FileSystemHelper.CreateFolder(Path.Combine(coreProjectPath, serviceName), "Services");
        FileSystemHelper.CreateFolder(Path.Combine(coreProjectPath, $"{serviceName}/Services"), "Impl");
        FileSystemHelper.CreateFolder(Path.Combine(coreProjectPath, serviceName), "Repositories");
        FileSystemHelper.CreateFolder(Path.Combine(coreProjectPath, serviceName), "Validators");

        GenerateCoreArtifacts(coreProjectPath, projectName, serviceName);
    }
    
    private static void GenerateCoreArtifacts(string coreProjectPath, string projectName, string serviceName)
    {
        var modelNameSpace = $"{projectName}.{serviceName}.Models";
        var interfaceServiceNameSpace = $"{projectName}.{serviceName}.Services";
        var serviceNameSpace = $"{projectName}.{serviceName}.Services.Impl";
        var repositoryNameSpace = $"{projectName}.{serviceName}.Repositories";
        var artifactName = serviceName.Singularize();
        
        var content = ArtifactsTemplates.GenerateClassTemplate(modelNameSpace, artifactName);
        var contentIService = ArtifactsTemplates.GenerateInterfaceTemplate(interfaceServiceNameSpace, artifactName, modelNameSpace);
        var contentService = ArtifactsTemplates.GenerateServiceTemplate(serviceNameSpace, artifactName, modelNameSpace);

        var classFilePath = Path.Combine(coreProjectPath, serviceName, "Models", $"{artifactName}.cs");
        var serviceInterfaceFilePath = Path.Combine(coreProjectPath, serviceName, "Services", $"I{artifactName}Service.cs");
        var serviceFilePath = Path.Combine(coreProjectPath, serviceName, "Services", "Impl", $"{artifactName}Service.cs");
        

        File.WriteAllText(classFilePath, content);
        File.WriteAllText(serviceInterfaceFilePath, contentIService);
        File.WriteAllText(serviceFilePath, contentService);
    }
    
}