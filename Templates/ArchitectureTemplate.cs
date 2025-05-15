using ApiGenerator.DotNet.Common;

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
}