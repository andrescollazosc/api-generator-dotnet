namespace ApiGenerator.DotNet.Scaffolding;

public static class ArtifactsTemplates
{
    public static string GenerateClassTemplate(string nameSpacePath, string className)
    {
        var classContent = $"namespace {nameSpacePath};\n" +
                           $"\npublic class {className}\n" +
                           "{\n" +
                           "\tpublic Guid Id { get; set; }\n" +
                           "}\n";

        return classContent;
    }
    
    public static string GenerateInterfaceTemplate(string nameSpacePath, string interfaceName, string usingModels)
    {
        var interfaceContent = $"using {usingModels};\n" +
                           $"\nnamespace {nameSpacePath};\n" +
                           $"\npublic interface I{interfaceName}Service\n" +
                           "{\n" +
                           $"\tTask<{interfaceName}> GetById(Guid id);\n" +
                           $"\tTask<IEnumerable<{interfaceName}>> GetAll();\n" +
                           $"\tTask Create({interfaceName} entity);\n" +
                           $"\tTask Update({interfaceName} entity);\n" +
                           $"\tTask Delete(Guid id);\n" +
                           "}\n";

        return interfaceContent;
    }
    
    public static string GenerateServiceTemplate(string nameSpacePath, string interfaceName, string usingModels)
    {
        var interfaceContent = $"using {usingModels};\n" +
                               $"\nnamespace {nameSpacePath};\n" +
                               $"\npublic class {interfaceName}Service : I{interfaceName}Service\n" +
                               "{\n" +
                               $"\tpublic async Task<{interfaceName}> GetById(Guid id)\n" +
                               "\t{\n" +
                               $"\t\tthrow new NotImplementedException();\n" +
                               "\t}\n\n" +
                               $"\tpublic async Task<IEnumerable<{interfaceName}>> GetAll()\n" +
                               "\t{\n" +
                               $"\t\tthrow new NotImplementedException();\n" +
                               "\t}\n\n" +
                               $"\tpublic async Task Create({interfaceName} entity)\n" +
                               "\t{\n" +
                               $"\t\tthrow new NotImplementedException();\n" +
                               "\t}\n\n" +
                               $"\tpublic async Task Update({interfaceName} entity)\n" +
                               "\t{\n" +
                               $"\t\tthrow new NotImplementedException();\n" +
                               "\t}\n\n" +
                               $"\tpublic async Task Delete(Guid id)\n" +
                               "\t{\n" +
                               $"\t\tthrow new NotImplementedException();\n" +
                               "\t}\n\n" +
                               "}\n";

        return interfaceContent;
    }
}