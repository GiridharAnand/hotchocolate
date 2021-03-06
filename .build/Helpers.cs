using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

class Helpers
{
    private static string[] _directories = new string[]
    {
        "GreenDonut",
        Path.Combine("HotChocolate", "AspNetCore"),
        Path.Combine("HotChocolate", "Core"),
        Path.Combine("HotChocolate", "Language"),
        Path.Combine("HotChocolate", "PersistedQueries"),
        Path.Combine("HotChocolate", "Utilities")
    };

    public static IEnumerable<string> GetAllProjects(string sourceDirectory)
    {
        foreach (string directory in _directories)
        {
            string fullDirectory = Path.Combine(sourceDirectory, directory);
            foreach (string file in Directory.EnumerateFiles(fullDirectory, "*.csproj", SearchOption.AllDirectories))
            {
                if (file.Contains("benchmark", StringComparison.OrdinalIgnoreCase) 
                    || file.Contains("HotChocolate.Core.Tests", StringComparison.OrdinalIgnoreCase)
                    || file.Contains("HotChocolate.Utilities.Introspection.Tests", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                yield return file;
            }
        }
    }

    public static IReadOnlyCollection<Output> DotNetBuildSonarSolution(
        string solutionFile)
    {
        if (File.Exists(solutionFile))
        {
            return Array.Empty<Output>();
        }

        IEnumerable<string> projects = GetAllProjects(Path.GetDirectoryName(solutionFile));
        var workingDirectory = Path.GetDirectoryName(solutionFile);
        var list = new List<Output>();

        list.AddRange(DotNetTasks.DotNet($"new sln -n {Path.GetFileNameWithoutExtension(solutionFile)}", workingDirectory));

        var projectsArg = string.Join(" ", projects.Select(t => $"\"{t}\""));

        list.AddRange(DotNetTasks.DotNet($"sln \"{solutionFile}\" add {projectsArg}", workingDirectory));

        return list;
    }
}
