using Spectre.Console;

namespace ApiGenerator.DotNet.Common.Helpers;

public static class FileSystemHelper
{
    public static void CreateFolder(string rootFolder, string folderName)
    {
        var newFolder = Path.Combine(rootFolder, folderName);
        Directory.CreateDirectory(newFolder);
        AnsiConsole.MarkupLine($"[green]📁 Folder created:[/] [yellow]{folderName}[/]");
    }
}