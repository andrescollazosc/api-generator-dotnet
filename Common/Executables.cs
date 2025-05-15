using System.Diagnostics;
using Spectre.Console;

namespace ApiGenerator.DotNet.Common;

public class Executables
{
    public static void RunCommand(string command, string workingDirectory)
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